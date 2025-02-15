using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace DatsuEditor.Controls.Common
{
    // Addiotinal code for map handling because it can take a lot of space
    public partial class PropertyEditor
    {
        public FrameworkElement GetMapValueEditor(Dictionary<string, string> Dict, KeyValuePair<string, string> KV)
        {
            var Value = new TextBox();
            Value.TextChanged += (s, e) => Dict[KV.Key] = Value.Text;
            Value.Text = Dict[KV.Key];
            return Value;
        }
    }
}
