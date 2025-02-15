using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using DatsuEditor.Controls.Editors.Bxm;
using DatsuEditor.Nodes;
using ZanLibrary.Formats.Common;

namespace DatsuEditor.Controls.Editors
{
    /// <summary>
    /// Interaction logic for BinaryTreeEditor.xaml
    /// </summary>
    public partial class BxmEditor : UserControl
    {
        BxmNode TargetFile;
        public BxmEditor()
        {
            InitializeComponent();
            MouseLeftButtonUp += OnNodeClick;
        }

        private void OnNodeClick(object sender, MouseButtonEventArgs e)
        {
            if (BxmTreeView.SelectedItem != null)
                DatsuEvents.RequestPropertyEditor(BxmTreeView.SelectedItem);
        }

        void RebuildBinary(object s, EventArgs e)
        {
            if (!BxmTreeView.HasItems)
                return;

            void FillChildren(BxmDataNode data, BxmEditorNode node)
            {
                foreach (BxmEditorNode child in node.Items)
                {
                    BxmDataNode dataNode = new();
                    dataNode.Value = child.Value;
                    dataNode.Attributes = child.Attributes;
                    dataNode.Name = child.ElementName;
                    dataNode.Children = new();
                    data.Children.Add(dataNode);
                    FillChildren(data, child);
                }
            }
            BxmDataNode dataNode = new();
            var root = (BxmEditorNode)BxmTreeView.Items[0];
            dataNode.Value = root.Value;
            dataNode.Attributes = root.Attributes;
            dataNode.Name = root.ElementName;
            dataNode.Children = new();
            FillChildren(dataNode, root);
            TargetFile.Data = BxmFile.Save(dataNode);
        }

        public void LoadBxm(BxmNode Node)
        {
            if (Node == null) return;

            TargetFile = Node;
            var data = BxmFile.Load(Node.Data);

            BxmTreeView.Items.Clear();

            void PopulateNode(BxmDataNode node, BxmEditorNode treenode)
            {
                foreach (var child in node.Children)
                {
                    BxmEditorNode childNode = new BxmEditorNode();
                    childNode.Value = child.Value;
                    childNode.Attributes = child.Attributes;
                    childNode.ElementName = child.Name;
                    childNode.OnBinaryUpdate += RebuildBinary;
                    PopulateNode(child, childNode);
                    childNode.UpdateNode();
                    treenode.Items.Add(childNode);
                }
            }

            var RootNodes = data.Where(x => x.Parent == null).ToList();
            foreach (var root in RootNodes)
            {
                BxmEditorNode RootNode = new();
                RootNode.Value = root.Value;
                RootNode.Attributes = root.Attributes;
                RootNode.ElementName = root.Name;
                RootNode.OnBinaryUpdate += RebuildBinary;
                PopulateNode(root, RootNode);
                RootNode.UpdateNode();
                BxmTreeView.Items.Add(RootNode);
            }
        }
    }
}
