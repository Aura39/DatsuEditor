using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace DatsuEditor
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            Font = SystemFonts.MessageBoxFont;
            InitializeComponent();

            var TextStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DatsuEditor.Credits.txt");
            byte[] TextBytes = new byte[TextStream.Length];
            TextStream.Read(TextBytes, 0, (int)TextStream.Length);

            CreditsBox.Text = Encoding.UTF8.GetString(TextBytes);

            var LicStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DatsuEditor.License.txt");
            byte[] LicBytes = new byte[LicStream.Length];
            LicStream.Read(LicBytes, 0, (int)LicStream.Length);

            LicenseBox.Text = Encoding.UTF8.GetString(LicBytes);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
