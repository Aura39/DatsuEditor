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
                OnBinaryUpdate?.Invoke(null, new EventArgs());
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
                OnBinaryUpdate?.Invoke(null, new EventArgs());
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
                OnBinaryUpdate?.Invoke(null, new EventArgs());
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
        }
    }
}
