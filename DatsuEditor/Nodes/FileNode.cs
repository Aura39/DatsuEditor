using System;
using System.IO;
using System.Windows.Forms;

namespace DatsuEditor.Nodes
{
    public class FileNode : TreeNode
    {
        public byte[] Data { get; set; }
        public string ExportFilter { get; set; }
        public string ReplaceFilter { get; set; }
        public event EventHandler OnReplace;
        public event EventHandler OnRename;
        public event EventHandler OnRemove;
        protected void CallOnRename()
        {
            OnRename?.Invoke(this, new EventArgs());
        }
        public FileNode(string filename, byte[] filedata) {
            ExportFilter = "All Files (*.*)|*.*";
            ReplaceFilter = "All Files (*.*)|*.*";

            Text = filename;  
            Data = filedata;
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
                    OnReplace?.Invoke(this, new EventArgs());
                }
            };

            var renameBtn = new ToolStripMenuItem("Rename");
            renameBtn.Click += (sender, args) =>
            {
                NodeRenamer nodeRenamer = new NodeRenamer(Text);
                nodeRenamer.OnRename += (s,e) =>
                {
                    Text = nodeRenamer.Result;
                };
                nodeRenamer.ShowDialog();
                OnRename?.Invoke(this, new EventArgs());
            };
            

            var removeBtn = new ToolStripMenuItem("Remove");
            removeBtn.Click += (sender, args) =>
            {
                Remove();
                OnRemove?.Invoke(this, new EventArgs());
            };
            ContextMenuStrip.Items.Add(exportBtn);
            ContextMenuStrip.Items.Add(replaceBtn);
            ContextMenuStrip.Items.Add(renameBtn);
            ContextMenuStrip.Items.Add(removeBtn);
        }
    }
}
