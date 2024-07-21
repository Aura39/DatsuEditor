using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DatsuEditor.Editors.Bxm
{
    public class BxmNameEditEventArgs : EventArgs
    {
        public BxmNameEditEventArgs(string name)
        {
            NodeName = name;
        }

        public string NodeName { get; set; }
    }
    class EditableBxmNode
    {
        private string _name;
        public string Name {
            get
            {
                return _name;
            }
            set {
                _name = value;
                OnNameChange?.Invoke(this, new BxmNameEditEventArgs(value));
            }
        }
        public string Value { get; set; }
        public Dictionary<string,string> Attributes { get; set; }
        public event EventHandler<BxmNameEditEventArgs> OnNameChange;
    }
    class BxmEditorNode : TreeNode
    {
        public EditableBxmNode BxmData;

        public BxmEditorNode(string name, string value, Dictionary<string,string> attributes)
        {
            Text = name;

            BxmData = new EditableBxmNode();
            BxmData.Name = name;
            BxmData.Value = value;
            BxmData.Attributes = attributes;
            BxmData.OnNameChange += (s,e) =>
            {
                Text = e.NodeName;
            };
        }
    }
}
