using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DatsuEditor.Common;
using Microsoft.Win32;

namespace DatsuEditor.Nodes
{
    public class FileNode : TreeViewItem
    {
        public byte[] Data { get; set; }
        public string ExportFilter { get; set; }
        public string ReplaceFilter { get; set; }
        public event EventHandler OnReplace;
        public event EventHandler OnRename;
        public event EventHandler OnRemove;

        public bool MarkForRemoval = false;

        private BitmapImage _icon;
        public BitmapImage NodeIcon { get { return _icon; } set { _icon = value; } }

        public void SetText(string text)
        {

        }

        protected void CallOnRename()
        {
            OnRename?.Invoke(this, new EventArgs());
        }
        public FileNode(string filename, byte[] filedata) {
            ExportFilter = "All Files (*.*)|*.*";
            ReplaceFilter = "All Files (*.*)|*.*";

            Style = (Style)Application.Current.MainWindow.Resources["FileNodeStyle"];
            _icon = DatsuResourceLoader.Icons["unk.png"];
            Header = filename;  
            Data = filedata;
            ContextMenu = new ContextMenu();

            var nameItem = new MenuItem();
            var label = new TextBlock();
            nameItem.IsEnabled = false;
            label.Text = filename;
            nameItem.Header = label;

            var exportBtn = new MenuItem();
            exportBtn.Header = "Export";
            exportBtn.Click += (sender, args) =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = Header.ToString();
                saveFileDialog.Filter = ExportFilter;
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, Data);
                }
            };

            var replaceBtn = new MenuItem();
            replaceBtn.Header = "Replace";
            replaceBtn.Click += (sender, args) =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.FileName = Header.ToString();
                ofd.Filter = ReplaceFilter;
                if (ofd.ShowDialog() == true)
                {
                    Data = File.ReadAllBytes(ofd.FileName);
                    OnReplace?.Invoke(this, new EventArgs());
                }
            };

            var renameBtn = new MenuItem();
            renameBtn.Header = "Rename";
            renameBtn.Click += (sender, args) =>
            {
                var nameBackup = Header.ToString();
                var textbox = new TextBox();
                textbox.Text = Header.ToString();
                Header = textbox;
                textbox.Focus();
                textbox.KeyDown += (sender, e) =>
                {
                    if (e.Key == System.Windows.Input.Key.Enter)
                    {
                        RenameValidity();
                    }
                };
                textbox.LostFocus += (s, e) =>
                {
                    RenameValidity();
                };
                void RenameValidity()
                {
                    if (Path.GetExtension(textbox.Text) == string.Empty)
                    {
                        MessageBox.Show(
                            "Extension not specified! Rename aborted.",
                            "No extension!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                        Header = nameBackup;
                        label.Text = nameBackup;
                    }
                    else
                    {
                        Header = textbox.Text;
                        label.Text = textbox.Text;
                        OnRename?.Invoke(this, new EventArgs());
                    }
                }
            };
            

            var removeBtn = new MenuItem();
            removeBtn.Header = "Remove";
            removeBtn.Click += (sender, args) =>
            {
                OnRemove?.Invoke(this, new EventArgs());
            };

            ContextMenu.Items.Add(nameItem);
            ContextMenu.Items.Add(new Separator());
            ContextMenu.Items.Add(exportBtn);
            ContextMenu.Items.Add(replaceBtn);
            ContextMenu.Items.Add(renameBtn);
            ContextMenu.Items.Add(removeBtn);
        }
    }
}
