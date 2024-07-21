using System.Drawing;
using System.Windows.Forms;

namespace DatsuEditor
{
    internal class DatsuDarkRenderer : ToolStripProfessionalRenderer
    {
        public DatsuDarkRenderer(ProfessionalColorTable table) : base(table)
        {
                
        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = ThemeDefines.Dark.Text;
            base.OnRenderItemText(e);
        }
    }
    internal class DatsuDarkColorTable : ProfessionalColorTable
    {
        public override Color ToolStripDropDownBackground => ThemeDefines.Dark.Background;
        public override Color ImageMarginGradientBegin => ThemeDefines.Dark.Background;
        public override Color ImageMarginGradientMiddle => ThemeDefines.Dark.Background;
        public override Color ImageMarginGradientEnd => ThemeDefines.Dark.Background;
        public override Color MenuItemSelected => ThemeDefines.Dark.Selection;
        public override Color MenuItemSelectedGradientBegin => ThemeDefines.Dark.Selection;
        public override Color MenuItemSelectedGradientEnd => ThemeDefines.Dark.Selection;
        public override Color MenuItemPressedGradientBegin => ThemeDefines.Dark.Background;
        public override Color MenuItemPressedGradientEnd => ThemeDefines.Dark.Background;
    }
}
