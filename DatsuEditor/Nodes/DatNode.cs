using System;
using System.Collections.Generic;
using ZanLibrary;
using System.IO;
using ZanLibrary.Formats.Container;
using System.Windows.Controls;
using Microsoft.Win32;
using DatsuEditor.Common;

namespace DatsuEditor.Nodes
{
    class DatNode : FileNode
    {
        public event EventHandler OnAdd;
        public event EventHandler OnBinaryRefresh;
        public Endianness Endian;
        public DatNode(string filename, byte[] filedata, bool nested) : base(filename, filedata)
        {

            Header = filename;
            Data = filedata;

            NodeIcon = DatsuResourceLoader.Icons["dat.png"];

            var addButton = new MenuItem();
            addButton.Header = "Add File(s)";
            addButton.Click += (s, e) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "All Files (*.*)|*.*";
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string fn in openFileDialog.FileNames)
                    {
                        byte[] data = File.ReadAllBytes(fn);
                        string fileName = Path.GetFileName(fn);
                        FileNode node = NewTypedNode(new DatFileEntry(fileName, data));
                        Items.Add(node);
                    }
                    UpdateBinary();
                    OnAdd?.Invoke(this, new EventArgs());                        
                }
            };

            if (!nested)
            {
                ContextMenu = new ContextMenu();
            }
            else
            {
                ContextMenu.Items.Add(new Separator());
            }
            ContextMenu.Items.Add(addButton);


            var Load = DatFile.Load(filedata, false);
            Endian = Load.Endian;
            InitNode(Load.Files);
        }
        public void UpdateBinary()
        {
            List<DatFileEntry> files = new List<DatFileEntry>();
            foreach (FileNode node in Items)
                files.Add(new DatFileEntry(node.Header.ToString(), node.Data));
            DatLoadResult dat = new DatLoadResult();
            dat.Files = files.ToArray();
            dat.Endian = Endian;
            Data = DatFile.Save(dat);
            OnBinaryRefresh?.Invoke(this, new EventArgs());
        }
        FileNode NewTypedNode(DatFileEntry file)
        {
            FileNode node;
            switch (FormatUtils.DetectFileFormat(file.Data))
            {
                case MGRFileFormat.DAT:
                    node = new DatNode(file.Name, file.Data, true);
                    ((DatNode)node).OnAdd += (s, e) => { UpdateBinary(); };
                    ((DatNode)node).OnBinaryRefresh += (s, e) => { UpdateBinary(); };
                    var Dat = DatFile.Load(file.Data);
                    ((DatNode)node).Endian = Dat.Endian;
                    ((DatNode)node).InitNode(Dat.Files);
                    break;
                case MGRFileFormat.BXM:
                    node = new BxmNode(file.Name, file.Data);
                    break;
                case MGRFileFormat.WTB:
                    node = new WtbNode(file.Name, file.Data);
                    break;
                case MGRFileFormat.WMB:
                    node = new WmbNode(file.Name, file.Data);
                    break;
                case MGRFileFormat.MOT:
                    node = new MotNode(file.Name, file.Data);
                    break;
                case MGRFileFormat.EST:
                    node = new EstNode(file.Name, file.Data);
                    break;
                case MGRFileFormat.BNK:
                    node = new BnkNode(file.Name, file.Data);
                    break;
                case MGRFileFormat.SCR:
                    node = new ScrNode(file.Name, file.Data);
                    break;
                case MGRFileFormat.WEM:
                    node = new WemNode(file.Name, file.Data);
                    break;
                case MGRFileFormat.CTX:
                    node = new CtxNode(file.Name, file.Data);
                    ((CtxNode)node).OnBinaryRefresh += (s, e) => { UpdateBinary(); };
                    break;
                default:
                    node = new FileNode(file.Name, file.Data);
                    break;
            }
            return node;
        }
        
        public void InitNode(DatFileEntry[] Files)
        {
            Items.Clear();
            FileNode[] collectedNodes = new FileNode[Files.Length];
            for (int i = 0; i < Files.Length; i++)
            {
                DatFileEntry file = Files[i];
                FileNode node = NewTypedNode(file);
                node.OnReplace += (s, e) => { UpdateBinary(); };
                node.OnRename += (s, e) => { UpdateBinary(); };
                node.OnRemove += (s, e) => { Items.Remove(node);  UpdateBinary(); };
                collectedNodes[i] = node;
            }
            foreach (var node in collectedNodes)
                Items.Add(node);
        }
    }
}
