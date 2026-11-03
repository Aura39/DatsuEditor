using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DatsuEditor.Common;
using ZanLibrary.Formats.Common;

namespace DatsuEditor.Controls.Editors.Bxm
{
    internal class BxmEditorNode : TreeViewItem
    {
        public Dictionary<string, string> _attributes;
        public event EventHandler OnBinaryUpdate;
        public event EventHandler OnRemove;

        public void RequestRegen()
        {
            OnBinaryUpdate?.Invoke(this, new());
        }

        [EditableProperty]
        public Dictionary<string, string> Attributes
        {
            get
            {
                return _attributes;
            }
            set
            {
                _attributes = value;
            }
        }

        string _value;

        [EditableProperty]
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                UpdateNode();
            }
        }

        string _elementName;

        [EditableProperty]
        public string ElementName
        {
            get
            {
                return _elementName;
            }
            set
            {
                _elementName = value;
                UpdateNode();
            }
        }

        public void UpdateNode()
        {
            var label = new TextBlock();
            var labelv = new TextBlock();
            label.Text = ElementName;
            labelv.Text = Value;
            labelv.Style = (Style)Application.Current.MainWindow.Resources["TipText"];

            var stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            stack.Children.Add(label);
            stack.Children.Add(labelv);
            Header = stack;
        }

        public BxmEditorNode()
        {
            Style = (Style)Application.Current.MainWindow.Resources["TreeNodeStyle"];
            ContextMenu = new ContextMenu();

            var addChildButton = new MenuItem();
            addChildButton.Header = "Add Child";
            addChildButton.Click += (s, e) =>
            {
                var node = new BxmEditorNode();
                node.Attributes = new();
                node.ElementName = "New Node";
                node.Value = "";
                node.OnRemove += (s,e) => { Items.Remove(node); };
                Items.Add(node);
            };

            var deleteButton = new MenuItem();
            deleteButton.Header = "Remove";
            deleteButton.Click += (s, e) =>
            {
                OnRemove?.Invoke(this, new());
            };

            var dupeButton = new MenuItem();
            dupeButton.Header = "Duplicate";
            dupeButton.Click += (s, e) =>
            {
                if (Parent != null)
                {
                    BxmEditorNode CloneNode(BxmEditorNode reference)
                    {
                        var node = new BxmEditorNode();
                        node.Attributes = reference.Attributes.ToDictionary();
                        node.Value = reference.Value is not null ? (string)reference.Value.Clone() : string.Empty;
                        node.ElementName = reference.ElementName is not null ? (string)reference.ElementName.Clone() : string.Empty;
                        foreach (var child in reference.Items)
                        {
                            node.Items.Add(CloneNode((BxmEditorNode)child));
                        }
                        node.UpdateNode();
                        return node;
                    }
                    ((BxmEditorNode)Parent).Items.Add(CloneNode(this));
                }
            };

            ContextMenu.Items.Add(addChildButton);
            ContextMenu.Items.Add(deleteButton);
            ContextMenu.Items.Add(dupeButton);
        }
    }
}
