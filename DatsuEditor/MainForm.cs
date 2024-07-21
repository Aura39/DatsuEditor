using DatsuEditor;
using DatsuEditor.Nodes;
using ZanLibrary.Dat;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using ZanLibrary;
using System.IO;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using DatsuEditor.Editors;
using System.ComponentModel;

namespace DatsuEditor
{
    public partial class MainForm : Form
    {
        TreeNode RootNode;
        int RecentFileAmount = 5;
        string[] RecentFiles = new string[5];
        string ConfigPath = Path.GetDirectoryName(
            Application.ExecutablePath
        ) + "\\" + "DatsuConfig.cfg";

        public const string DatFileFilter = "Data Container Files (*.dat)|*.dat|Texture Container Files (*.dtt)|*.dtt|Event Container Files (*.evn)|*.evn|Effect Container Files (*.eff)|*.eff|Effect Texture Container Files (*.eft)|*.eft";

        // Loaded stuff
        public static string LoadedFilePath = "";
        public static byte[] LoadedData = { };

        private void RequestFileEditor(MGRFileFormat type, FileNode caller)
        {
            switch (type)
            {
                case MGRFileFormat.EST:
                    SplitPreviewTool.Panel1.Controls.Clear();
                    var ested = new EstEditor(caller);
                    ested.Dock = DockStyle.Fill;
                    SplitPreviewTool.Panel1.Controls.Add(ested);
                    break;
                case MGRFileFormat.BXM:
                    SplitPreviewTool.Panel1.Controls.Clear();
                    var bxmed = new BxmEditor(caller, this);
                    bxmed.Dock = DockStyle.Fill;
                    SplitPreviewTool.Panel1.Controls.Add(bxmed);
                    break;
            }
        }

        public void AddRecentEntry(string path)
        {
            if (RecentFiles == null) return;
            Array.Copy(RecentFiles, 0, RecentFiles, 1, RecentFiles.Length - 1);
            RecentFiles[0] = path;
            UpdateRecentFiles(); 
        }

        public MainForm(string StartupFile)
        {
            Font = SystemFonts.MessageBoxFont;
            InitializeComponent();

            // DatsuGlobals.NodeView = FileTree;

            DatsuGlobals.OnThemeSwitch += UpdateTheme;
            DatsuGlobals.OnStatusUpdate += (s,e) =>
            {
                StatusMessage.Text = e.Message;
            };

            byte[] IconBytes = new byte[1];
            if (Environment.OSVersion.Version.Major >= 6)
            {
                var IconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DatsuEditor.DatsuIcons.dat");
                IconBytes = new byte[IconStream.Length];
                IconStream.Read(IconBytes, 0, (int)IconStream.Length);
            }
            else
            {
                IconBytes = File.ReadAllBytes("DatsuIcons.dat");
            }

            var IconPackage = DatFile.Load(IconBytes);

            Dictionary<string, MemoryStream> IconMap = new Dictionary<string, MemoryStream>();

            foreach (var IconFile in IconPackage)
            {
                IconMap.Add(IconFile.Name, new MemoryStream(IconFile.Data));
            }

            ImageList list = new ImageList();
            var test = IconMap["unk.png"];
            var testimage = Image.FromStream(IconMap["unk.png"]);
            list.Images.Add(Image.FromStream(IconMap["unk.png"]));
            list.Images.Add(Image.FromStream(IconMap["dat.png"]));
            list.Images.Add(Image.FromStream(IconMap["bxm.png"]));
            list.Images.Add(Image.FromStream(IconMap["wtb.png"]));
            list.Images.Add(Image.FromStream(IconMap["wmb.png"]));
            list.Images.Add(Image.FromStream(IconMap["mot.png"]));
            list.Images.Add(Image.FromStream(IconMap["est.png"]));
            list.Images.Add(Image.FromStream(IconMap["bnk.png"]));
            list.Images.Add(Image.FromStream(IconMap["scr.png"]));
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // syn
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // ly2
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // uid
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // sop
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // exp
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // ctx
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // uvd
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // sae
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // sas
            list.Images.Add(Image.FromStream(IconMap["hkx.png"])); // hkx
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // cpk
            list.Images.Add(Image.FromStream(IconMap["wem.png"]));
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // vcd
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // brd
            list.Images.Add(Image.FromStream(IconMap["unk.png"])); // mcd
            FileTree.ImageList = list;

            if (File.Exists(ConfigPath))
                LoadConfig();

            UpdateRecentFiles();

            if (RecentFiles is null)
                RecentFiles = new string[5];

            UpdateTheme( null, new ThemeSwitchEventArgs(DatsuGlobals.SelectedTheme) );

            if (StartupFile != null)
                TryLoadFile(StartupFile);
        }

        void UpdateRecentFiles()
        {
            OpenRecentMenuItem.DropDownItems.Clear();
            foreach (var filepath in RecentFiles)
            {
                if (filepath != null)
                {
                    ToolStripMenuItem recentItem = new ToolStripMenuItem(filepath);
                    recentItem.Click += (s,e) =>
                    {
                        TryLoadFile(recentItem.Text);
                    };
                    OpenRecentMenuItem.DropDownItems.Add(recentItem);
                }
            }
        }

        void SaveConfig()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == ConfigPath.Substring(0,3))
                {
                    if (drive.AvailableFreeSpace < 1024)
                    {
                        MessageBox.Show($"Insufficent drive space on \"{drive.Name}\" to save config! Clean up your drive!");
                        return;
                    }
                }
            }

            StringBuilder configFile = new StringBuilder();
            configFile.AppendLine("Theme=" + (((int)DatsuGlobals.SelectedTheme)));
            configFile.AppendLine("RecentAmount=" + RecentFileAmount);
            for (int i = 0; i < RecentFiles.Length; i++)
            {
                configFile.AppendLine($"RecentFileEntry.{i}={RecentFiles[i]}");

            }

            File.WriteAllText(ConfigPath, configFile.ToString());
        }

        void LoadConfig()
        {
            var lines = File.ReadAllLines(ConfigPath);
            foreach (var line in lines)
            {
                string[] split = line.Split('=');
                switch (split[0])
                {
                    case "Theme":
                        try
                        {
                            DatsuGlobals.SelectedTheme = (Theme)int.Parse(split[1]);
                        }
                        catch (Exception e)
                        {
                            DatsuGlobals.SelectedTheme = Theme.Dark;
                        }
                        break;
                    case "RecentAmount":
                        RecentFileAmount = int.Parse(split[1]);
                        RecentFiles = new string[RecentFileAmount];
                        break;
                }
                if (split[0].StartsWith("RecentFileEntry"))
                {
                    var Split = split[0].Split('.');
                    int fileEntryId = int.Parse(Split[1]);
                    if (fileEntryId < RecentFiles.Length)
                        if (split[1] != string.Empty)
                            RecentFiles[fileEntryId] = split[1];
                }
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy | DragDropEffects.Move;
            TryLoadFile(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
        }

        private void TryLoadFile(string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("Error loading file: File at specified path does not exist.");
                DatsuGlobals.UpdateStatus($"Error loading \"{Path.GetFileName(path)}\" - No such file.");
                return;
            }
            string filename = Path.GetFileName(path);

            byte[] data = File.ReadAllBytes(path);

            switch (FormatUtils.DetectFileFormat(data))
            {
                case MGRFileFormat.DAT:
                    try
                    {
                        Stopwatch timer = new Stopwatch();
                        timer.Start();

                        FileTree.BeginUpdate();
                        FileTree.Nodes.Clear();
                        var Files = DatFile.Load(data, false);
                        LoadedFilePath = path;
                        DatNode datNode = new DatNode(path, data, false);
                        FileTree.Nodes.Add(datNode);
                        datNode.InitNode(DatFile.Load(data));
                        datNode.Text = filename;
                        datNode.Expand();
                        FileTree.EndUpdate();
                        AddRecentEntry(path);

                        timer.Stop();
                        DatsuGlobals.UpdateStatus($"Loaded \"{filename}\" ({Files.Length} files) in {timer.ElapsedMilliseconds}ms.");
                        UpdateTitle();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reading DAT file: " + ex.Message);
                    }
                    return;
            }
        }

        private void DarkThemeButton_Click(object sender, EventArgs e)
        {
            DatsuGlobals.ChangeTheme(Theme.Dark);
        }

        private void LightThemeButton_Click(object sender, EventArgs e)
        {
            DatsuGlobals.ChangeTheme(Theme.Light);
        }

        void UpdateTitle()
        {
            Text = $"DatsuEditor - {LoadedFilePath}";
        }

        void UpdateTheme(object s, ThemeSwitchEventArgs e)
        {
            switch (e.Theme)
            {
                case Theme.Dark:
                    LightThemeButton.Checked = false;
                    DarkThemeButton.Checked = true;

                    BackColor = ThemeDefines.Dark.Baseground;
                    ForeColor = ThemeDefines.Dark.Text;

                    FileTree.BackColor = ThemeDefines.Dark.Background;
                    FileTree.ForeColor = ThemeDefines.Dark.Text;

                    PropertyGrid.BackColor = ThemeDefines.Dark.Background;
                    PropertyGrid.ViewBackColor = ThemeDefines.Dark.Background;
                    PropertyGrid.ViewForeColor = ThemeDefines.Dark.Text;

                    menuStrip1.Renderer = new DatsuDarkRenderer(new DatsuDarkColorTable());
                    menuStrip1.BackColor = ThemeDefines.Dark.Baseground;
                    menuStrip1.ForeColor = ThemeDefines.Dark.Text;

                    statusStrip1.BackColor = ThemeDefines.Dark.Background;
                    statusStrip1.ForeColor = ThemeDefines.Dark.Text;

                    ImmersiveWindow.ToggleWindowDark(Handle, true);
                    break;
                case Theme.Light:
                    LightThemeButton.Checked = true;
                    DarkThemeButton.Checked = false;

                    BackColor = SystemColors.Control;
                    ForeColor = SystemColors.ControlText;

                    FileTree.BackColor = SystemColors.Window;
                    FileTree.ForeColor = SystemColors.ControlText;

                    PropertyGrid.BackColor = SystemColors.Control;
                    PropertyGrid.ViewBackColor = SystemColors.Window;
                    PropertyGrid.ViewForeColor = SystemColors.ControlText;

                    menuStrip1.Renderer = new ToolStripProfessionalRenderer();
                    menuStrip1.BackColor = SystemColors.Control;
                    menuStrip1.ForeColor = SystemColors.ControlText;

                    statusStrip1.BackColor = SystemColors.Control;
                    statusStrip1.ForeColor = SystemColors.ControlText;

                    ImmersiveWindow.ToggleWindowDark(Handle, false);
                    break;
            }
            SaveConfig();
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog();
        }

        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            Stopwatch repackingTimer = new Stopwatch();
            repackingTimer.Start();
            List<DatFileEntry> files = new List<DatFileEntry>();
            foreach (FileNode node in FileTree.Nodes[0].Nodes)
                files.Add(new DatFileEntry(node.Text, node.Data));
            File.WriteAllBytes(LoadedFilePath, DatFile.Save(files.ToArray()));
            repackingTimer.Stop();
            DatsuGlobals.UpdateStatus($"Successfully repacked \"{Path.GetFileName(LoadedFilePath)}\" in {repackingTimer.ElapsedMilliseconds}ms.");
        }

        private void FileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileTree.Nodes.Clear();
            LoadedData = new byte[] { };
            LoadedFilePath = string.Empty;
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            // DatFile crasher = null;
            // crasher.GetType();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = DatFileFilter;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                TryLoadFile(ofd.FileName);
            }
        }

        private void ExitToolButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = DatFileFilter;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Stopwatch repackingTimer = new Stopwatch();
                repackingTimer.Start();
                List<DatFileEntry> files = new List<DatFileEntry>();
                foreach (FileNode node in FileTree.Nodes[0].Nodes)
                    files.Add(new DatFileEntry(node.Text, node.Data));
                File.WriteAllBytes(sfd.FileName, DatFile.Save(files.ToArray()));
                LoadedFilePath = sfd.FileName;
                repackingTimer.Stop();
                DatsuGlobals.UpdateStatus($"Successfully repacked \"{Path.GetFileName(LoadedFilePath)}\" in {repackingTimer.ElapsedMilliseconds}ms.");
                UpdateTitle();
            }
        }

        private void FileFilterBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void FileTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            RequestFileEditor(FormatUtils.DetectFileFormat(((FileNode)e.Node).Data), (FileNode)e.Node);
        }

        private void assumeConsoleFilesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            // DatsuGlobals.IsBigEndian = assumeConsoleFilesToolStripMenuItem.Checked;
        }

        private void FileTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}
