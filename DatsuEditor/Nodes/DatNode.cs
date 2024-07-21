using ZanLibrary.Dat;
using System;
using System.Collections.Generic;
using ZanLibrary;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;

namespace DatsuEditor.Nodes
{
    class DatNode : FileNode
    {
        public event EventHandler OnAdd;
        public event EventHandler OnBinaryRefresh;
        public DatNode(string filename, byte[] filedata, bool nested) : base(filename, filedata)
        {

            ContextMenuStrip = new ContextMenuStrip();

            var exportBtn = new ToolStripMenuItem("Export");
            exportBtn.Click += (sender, args) =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = Text;
                saveFileDialog.Filter = $"All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, Data);
                }
            };

            var replaceBtn = new ToolStripMenuItem("Replace");
            replaceBtn.Click += (sender, args) =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.FileName = Text;
                ofd.Filter = $"All Files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Data = File.ReadAllBytes(ofd.FileName);
                    InitNode(DatFile.Load(Data));
                }
            };

            var renameBtn = new ToolStripMenuItem("Rename");
            renameBtn.Click += (sender, args) =>
            {
                NodeRenamer nodeRenamer = new NodeRenamer(Text);
                nodeRenamer.OnRename += (s, e) =>
                {
                    Text = nodeRenamer.Result;
                };
                nodeRenamer.ShowDialog();
                CallOnRename();
            };

            var removeBtn = new ToolStripMenuItem("Remove");
            removeBtn.Click += (sender, args) =>
            {
                Remove();
            };

            var addButton = new ToolStripMenuItem("Add File(s)");
            addButton.Click += (s, e) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "All Files (*.*)|*.*";
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string fn in openFileDialog.FileNames)
                    {
                        byte[] data = File.ReadAllBytes(fn);
                        string fileName = Path.GetFileName(fn);
                        FileNode node = NewTypedNode(new DatFileEntry(fileName, data));
                        Nodes.Add(node);
                    }
                    UpdateBinary();
                    OnAdd?.Invoke(this, new EventArgs());                        
                }
            };
            
            var unpackButton = new ToolStripMenuItem("Extract All");
            unpackButton.Click += (s, e) =>
            {
                FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var Node in Nodes)
                        File.WriteAllBytes(((FileNode)Node).Name, ((FileNode)Node).Data);
                }
            };

            if (nested)
            {
                ContextMenuStrip.Items.Add(exportBtn);
                ContextMenuStrip.Items.Add(replaceBtn);
                ContextMenuStrip.Items.Add(renameBtn);
                ContextMenuStrip.Items.Add(removeBtn);
                ContextMenuStrip.Items.Add(new ToolStripSeparator());
            }
            ContextMenuStrip.Items.Add(addButton);
            ContextMenuStrip.Items.Add(unpackButton);
            ImageIndex = 1;
            SelectedImageIndex = 1;
        }
        public void UpdateBinary()
        {
            List<DatFileEntry> files = new List<DatFileEntry>();
            foreach (FileNode node in Nodes)
                files.Add(new DatFileEntry(node.Text, node.Data));
            Data = DatFile.Save(files.ToArray());
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
                    ((DatNode)node).InitNode(DatFile.Load(file.Data));
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
                default:
                    node = new FileNode(file.Name, file.Data);
                    break;
            }
            return node;
        }
        
        public void InitNode(DatFileEntry[] Files)
        {
            Nodes.Clear();
            FileNode[] collectedNodes = new FileNode[Files.Length];
            for (int i = 0; i < Files.Length; i++)
            {
                DatFileEntry file = Files[i];
                FileNode node = NewTypedNode(file);
                node.OnReplace += (s, e) => { UpdateBinary(); };
                node.OnRename += (s, e) => { UpdateBinary(); };
                node.OnRemove += (s, e) => { UpdateBinary(); };
                collectedNodes[i] = node;
            }
            Nodes.AddRange(collectedNodes);
        }
    }
}
