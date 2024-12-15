using DatsuEditor.Nodes;
using System.Windows.Forms;
using ZanLibrary.Formats.Rendering.Effect;

namespace DatsuEditor.Editors
{
    public partial class EstEditor : UserControl
    {
        FileNode parentNode;
        public EstEditor(FileNode caller)
        {
            InitializeComponent();

            parentNode = caller;
            label2.Text = $"{parentNode.Text}";

            var EffectData = EstFile.Load(caller.Data);

            foreach ( var record in EffectData.Records)
            {
                listBox1.Items.Add(record.Id);
            }
        }
    }
}
