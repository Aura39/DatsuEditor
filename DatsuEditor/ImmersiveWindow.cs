using System;
using System.Runtime.InteropServices;

namespace DatsuEditor
{
    public class ImmersiveWindow
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        
        public static void ToggleWindowDark(IntPtr hwnd, bool enabled)
        {
            int immersiveDarkMode = enabled ? 1 : 0;
            DwmSetWindowAttribute(hwnd, 20, ref immersiveDarkMode, sizeof(int));
        }
    }
}
