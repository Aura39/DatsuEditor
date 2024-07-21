using System.Drawing;

namespace DatsuEditor
{
    public class DatsuTheme {
        public Color Baseground;
        public Color Background;
        public Color Selection;
        public Color Text;
    }
    public class DarkTheme : DatsuTheme
    {
        public DarkTheme()
        {
            Baseground = Color.FromArgb(24,24,24);
            Background = Color.FromArgb(36,36,36);
            Selection = Color.FromArgb(36,56,96);
            Text = Color.FromArgb(255,255,255);            
        }
    }
    public class ThemeDefines
    {
        public static readonly DarkTheme Dark = new DarkTheme();
    }
}
