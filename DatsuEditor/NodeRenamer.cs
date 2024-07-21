using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DatsuEditor
{
    public partial class NodeRenamer : Form
    {
        public event EventHandler OnRename;
        public string Result = "";
        public NodeRenamer(string InitialName)
        {
            InitializeComponent();
            textBox1.Text = InitialName;
            Focus();
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Result = textBox1.Text;
            OnRename.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
