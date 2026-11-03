using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using ZanLibrary;
using ZanLibrary.Formats.Container;
using System.IO;
using DatsuEditor.Nodes;
using Microsoft.Win32;
using DatsuEditor.Common;
using DatsuEditor.Controls.Editors;

namespace DatsuEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, DatNode> LoadedArchives = new();
        public MainWindow()
        {
            DatsuResourceLoader.ReloadIcons();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            InitializeComponent();
            RegisterEvents();

            NodeView.MouseDoubleClick += NodeDoubleClick;
            NodeView.KeyDown += NodeViewKey;

#if DEBUG
            PropertyGrid.SetTarget(new PropertyGridTester());
#endif
        }

        private void NodeViewKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                NodeDoubleClick(null, null);
        }

        private void NodeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (NodeView.SelectedItem != null)
                DatsuEvents.RequestEditor((FileNode)NodeView.SelectedItem);
        }

        public void RegisterEvents()
        {
            DatsuEvents.OnStatusSubmit += UpdateStatus;
            DatsuEvents.OnEditorRequest += OnEditorRequest;
            DatsuEvents.OnRequestPropertyEditor += RequestPropertyEditor;
        }

        private void RequestPropertyEditor(object sender, ChangePropertyObjectEventArgs e)
        {
            PropertyGrid.SetTarget(e.Target);
        }

        private void OnEditorRequest(object sender, EditorRequestEventArgs e)
        {
            switch (e.TargetFile)
            {
                case BxmNode bxm:
                    var editor = new BxmEditor();
                    FileEditor.Content = editor;
                    editor.LoadFile(bxm);
                    break;
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            LoadedArchives.Clear();
            e.Effects = DragDropEffects.Copy | DragDropEffects.Move;
            string[] Files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (Files == null)
                return;

            foreach (string file in Files)
            {
                TryLoadFile(file, false);
            }
        }

        private void UpdateStatus(object sender, StatusEventArgs e)
        {
            StatusMessage.Text = e.Message;
        }

        private void TrySaveFiles(bool RequestDialog = false)
        {
            
            if (FileEditor.Content != null)
                ((FileEditor)FileEditor.Content).SaveFile();
            if (RequestDialog)
            {
                string filter = new FileFilterBuilder()
                    .AddFilters(["dat", "dtt", "evn", "eff", "eft"], "Data Archive")
                    .GetFilter();
                var DictCopy = LoadedArchives.ToDictionary();
                foreach (var ArchivePair in DictCopy)
                {
                    SaveFileDialog sfd = new();
                    sfd.InitialDirectory = System.IO.Path.GetDirectoryName(ArchivePair.Key);
                    sfd.FileName = System.IO.Path.GetFileName(ArchivePair.Key);
                    sfd.Filter = filter;
                    if (sfd.ShowDialog() == true)
                    {
                        var copy = LoadedArchives[ArchivePair.Key];
                        LoadedArchives.Remove(ArchivePair.Key);
                        LoadedArchives.Add(sfd.FileName, copy);
                        copy.Header = System.IO.Path.GetFileName(sfd.FileName);
                    }
                }
            }
            Stopwatch timer = new();
            timer.Start();
            foreach (var ArchivePair in LoadedArchives)
            {
                ArchivePair.Value.UpdateBinary();
                File.WriteAllBytes(ArchivePair.Key, ArchivePair.Value.Data);
            }
            timer.Stop();
            if (LoadedArchives.Count == 1)
                DatsuEvents.SubmitStatus($"Saved \"{Path.GetFileName(LoadedArchives.First().Key)}\" in {timer.ElapsedMilliseconds}ms");
            else
                DatsuEvents.SubmitStatus($"Saved {LoadedArchives.Count} files in {timer.ElapsedMilliseconds}ms");
        }

        private void TryLoadFile(string path, bool resetView = true)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("Error loading file: File at specified path does not exist.");
                DatsuEvents.SubmitStatus($"Error loading \"{System.IO.Path.GetFileName(path)}\" - No such file.");
                return;
            }
            string filename = System.IO.Path.GetFileName(path);

            byte[] data = File.ReadAllBytes(path);

            switch (FormatUtils.DetectFileFormat(data))
            {
                case MGRFileFormat.DAT:
                    try
                    {
                        Stopwatch timer = new Stopwatch();
                        timer.Start();
                        
                        if (resetView)
                            NodeView.Items.Clear();
                        DatNode datNode = new DatNode(path, data, false);
                        datNode.Header = filename;
                        NodeView.Items.Add(datNode);
                        LoadedArchives.TryAdd(path, datNode);

                        timer.Stop();

                        DatsuEvents.SubmitStatus($"File \"{filename}\" ({datNode.Items.Count} files) loaded in {timer.ElapsedMilliseconds}ms.");
                        FileEditor.Content = null;
                        DatsuEvents.RequestPropertyEditor(null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reading DAT file: " + ex.Message);
                    }
                    return;
            }
        }

        private void FileMenu_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string filter = new FileFilterBuilder()
                .AddFilters(["dat", "dtt", "evn", "eff", "eft"], "Data Archive")
                .GetFilter();

            ofd.Filter = filter;
            if (ofd.ShowDialog() == true)
            {
                LoadedArchives.Clear();
                TryLoadFile(ofd.FileName);
            }
        }

        private void FileMenu_Save_Click(object sender, EventArgs e)
        {
            TrySaveFiles();
        }

        private void FileMenu_SaveAs_Click(object sender, EventArgs e)
        {
            TrySaveFiles(true);
        }

        private void CloseArchive_Click(object sender, RoutedEventArgs e)
        {
            LoadedArchives.Clear();
            NodeView.Items.Clear();
        }
    }
}
