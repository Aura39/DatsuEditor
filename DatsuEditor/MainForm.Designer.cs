using DatsuEditor;
using System.Drawing;
using System.Windows.Forms;

namespace DatsuEditor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.SplitPreviewTool = new System.Windows.Forms.SplitContainer();
            this.SplitTreeProperty = new System.Windows.Forms.SplitContainer();
            this.FileTree = new System.Windows.Forms.TreeView();
            this.PropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenRecentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveFileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolButton = new System.Windows.Forms.ToolStripMenuItem();
            this.PreferencesMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.themeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LightThemeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DarkThemeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutButton = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.SplitPreviewTool.Panel2.SuspendLayout();
            this.SplitPreviewTool.SuspendLayout();
            this.SplitTreeProperty.Panel1.SuspendLayout();
            this.SplitTreeProperty.Panel2.SuspendLayout();
            this.SplitTreeProperty.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SplitPreviewTool
            // 
            this.SplitPreviewTool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SplitPreviewTool.Location = new System.Drawing.Point(9, 26);
            this.SplitPreviewTool.Name = "SplitPreviewTool";
            // 
            // SplitPreviewTool.Panel2
            // 
            this.SplitPreviewTool.Panel2.Controls.Add(this.SplitTreeProperty);
            this.SplitPreviewTool.Size = new System.Drawing.Size(870, 483);
            this.SplitPreviewTool.SplitterDistance = 505;
            this.SplitPreviewTool.SplitterWidth = 3;
            this.SplitPreviewTool.TabIndex = 0;
            // 
            // SplitTreeProperty
            // 
            this.SplitTreeProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitTreeProperty.Location = new System.Drawing.Point(0, 0);
            this.SplitTreeProperty.Name = "SplitTreeProperty";
            this.SplitTreeProperty.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitTreeProperty.Panel1
            // 
            this.SplitTreeProperty.Panel1.Controls.Add(this.FileTree);
            // 
            // SplitTreeProperty.Panel2
            // 
            this.SplitTreeProperty.Panel2.Controls.Add(this.PropertyGrid);
            this.SplitTreeProperty.Size = new System.Drawing.Size(362, 483);
            this.SplitTreeProperty.SplitterDistance = 233;
            this.SplitTreeProperty.SplitterWidth = 3;
            this.SplitTreeProperty.TabIndex = 0;
            // 
            // FileTree
            // 
            this.FileTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FileTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileTree.Location = new System.Drawing.Point(0, 0);
            this.FileTree.Name = "FileTree";
            this.FileTree.ShowNodeToolTips = true;
            this.FileTree.Size = new System.Drawing.Size(362, 233);
            this.FileTree.TabIndex = 0;
            this.FileTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FileTree_NodeMouseDoubleClick);
            this.FileTree.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FileTree_MouseDoubleClick);
            // 
            // PropertyGrid
            // 
            this.PropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGrid.HelpVisible = false;
            this.PropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.PropertyGrid.Name = "PropertyGrid";
            this.PropertyGrid.Size = new System.Drawing.Size(362, 247);
            this.PropertyGrid.TabIndex = 0;
            this.PropertyGrid.ToolbarVisible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.PreferencesMenuButton,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(887, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFileButton,
            this.OpenRecentMenuItem,
            this.SaveFileButton,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator1,
            this.ExitToolButton});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.ShortcutKeyDisplayString = "Ctrl+O";
            this.OpenFileButton.Size = new System.Drawing.Size(195, 22);
            this.OpenFileButton.Text = "Open";
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // OpenRecentMenuItem
            // 
            this.OpenRecentMenuItem.Name = "OpenRecentMenuItem";
            this.OpenRecentMenuItem.Size = new System.Drawing.Size(195, 22);
            this.OpenRecentMenuItem.Text = "Open Recent";
            // 
            // SaveFileButton
            // 
            this.SaveFileButton.Name = "SaveFileButton";
            this.SaveFileButton.ShortcutKeyDisplayString = "Ctrl+S";
            this.SaveFileButton.Size = new System.Drawing.Size(195, 22);
            this.SaveFileButton.Text = "Save";
            this.SaveFileButton.Click += new System.EventHandler(this.SaveFileButton_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+S";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+C";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
            // 
            // ExitToolButton
            // 
            this.ExitToolButton.Name = "ExitToolButton";
            this.ExitToolButton.ShortcutKeyDisplayString = "Alt+F4";
            this.ExitToolButton.Size = new System.Drawing.Size(195, 22);
            this.ExitToolButton.Text = "Exit";
            this.ExitToolButton.Click += new System.EventHandler(this.ExitToolButton_Click);
            // 
            // PreferencesMenuButton
            // 
            this.PreferencesMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.themeToolStripMenuItem});
            this.PreferencesMenuButton.Name = "PreferencesMenuButton";
            this.PreferencesMenuButton.Size = new System.Drawing.Size(61, 20);
            this.PreferencesMenuButton.Text = "Options";
            // 
            // themeToolStripMenuItem
            // 
            this.themeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LightThemeButton,
            this.DarkThemeButton});
            this.themeToolStripMenuItem.Name = "themeToolStripMenuItem";
            this.themeToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.themeToolStripMenuItem.Text = "Theme";
            // 
            // LightThemeButton
            // 
            this.LightThemeButton.Name = "LightThemeButton";
            this.LightThemeButton.Size = new System.Drawing.Size(101, 22);
            this.LightThemeButton.Text = "Light";
            this.LightThemeButton.Click += new System.EventHandler(this.LightThemeButton_Click);
            // 
            // DarkThemeButton
            // 
            this.DarkThemeButton.Name = "DarkThemeButton";
            this.DarkThemeButton.Size = new System.Drawing.Size(101, 22);
            this.DarkThemeButton.Text = "Dark";
            this.DarkThemeButton.Click += new System.EventHandler(this.DarkThemeButton_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutButton});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // AboutButton
            // 
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new System.Drawing.Size(171, 22);
            this.AboutButton.Text = "About DatsuEditor";
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 509);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(887, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusMessage
            // 
            this.StatusMessage.Name = "StatusMessage";
            this.StatusMessage.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.StatusMessage.Size = new System.Drawing.Size(874, 17);
            this.StatusMessage.Spring = true;
            this.StatusMessage.Text = "Drag and drop a file to start.";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(887, 531);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.SplitPreviewTool);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "DatsuEditor";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.SplitPreviewTool.Panel2.ResumeLayout(false);
            this.SplitPreviewTool.ResumeLayout(false);
            this.SplitTreeProperty.Panel1.ResumeLayout(false);
            this.SplitTreeProperty.Panel2.ResumeLayout(false);
            this.SplitTreeProperty.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SplitContainer SplitPreviewTool;
        private SplitContainer SplitTreeProperty;
        private TreeView FileTree;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem PreferencesMenuButton;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem OpenFileButton;
        private ToolStripMenuItem SaveFileButton;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem ExitToolButton;
        private ToolStripMenuItem themeToolStripMenuItem;
        private ToolStripMenuItem LightThemeButton;
        private ToolStripMenuItem DarkThemeButton;
        private ToolStripMenuItem AboutButton;
        private ToolStripSeparator toolStripSeparator1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel StatusMessage;
        private ToolStripMenuItem OpenRecentMenuItem;
        public PropertyGrid PropertyGrid;
    }
}
