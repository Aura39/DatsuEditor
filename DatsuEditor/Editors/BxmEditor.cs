using DatsuEditor.Editors.Bxm;
using DatsuEditor.Nodes;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZanLibrary.Formats.Common;

namespace DatsuEditor.Editors
{
    public partial class BxmEditor : UserControl
    {
        BxmNode parentNode;
        private void PopulateNode(BxmDataNode node, BxmEditorNode treenode)
        {
            foreach (var child in node.Children)
            {
                BxmEditorNode childNode = new BxmEditorNode(child.Name, child.Value, child.Attributes);
                PopulateNode(child, childNode);
                treenode.Nodes.Add(childNode);
            }
        }

        private void UpdateTheme(object s, ThemeSwitchEventArgs e)
        {
            switch (e.Theme)
            {
                case Theme.Dark:
                    bxmView.BackColor = ThemeDefines.Dark.Background;
                    bxmView.ForeColor = ThemeDefines.Dark.Text;
                    break;
                case Theme.Light:
                    bxmView.BackColor = SystemColors.Window;
                    bxmView.ForeColor = SystemColors.ControlText;
                    break;
            }
        }

        public BxmEditor(FileNode caller, MainForm mainEditor)
        {
            InitializeComponent();

            DatsuGlobals.OnThemeSwitch += UpdateTheme;
            UpdateTheme(null, new ThemeSwitchEventArgs(DatsuGlobals.SelectedTheme));

            parentNode = (BxmNode)caller;

            bxmView.NodeMouseClick += (s,e) =>
            {
                mainEditor.PropertyGrid.SelectedObject = ((BxmEditorNode)(e.Node)).BxmData;
                mainEditor.PropertyGrid.ResetSelectedProperty();
                mainEditor.PropertyGrid.Update();
            };

            var NodeData = BxmFile.Load(caller.Data);

            var RootBxmNode = NodeData.Where((x) => x.Parent == null).First();

            BxmEditorNode rootNode = new BxmEditorNode(RootBxmNode.Name, RootBxmNode.Value, RootBxmNode.Attributes);
            PopulateNode(RootBxmNode, rootNode);
            bxmView.Nodes.Add(rootNode);

            BxmFile.Save(RootBxmNode);
        }
    }
}
