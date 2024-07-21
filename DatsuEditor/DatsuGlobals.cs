using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DatsuEditor
{
    public enum Theme
    {
        Light = 0,
        Dark,
        Custom,
    }
    public class ThemeSwitchEventArgs : EventArgs
    {
        public ThemeSwitchEventArgs(Theme theme)
        {
            Theme = theme;
        }

        public Theme Theme { get; set; }
    }
    public class StatusUpdateEventArgs : EventArgs
    {
        public StatusUpdateEventArgs(string msg)
        {
            Message = msg;
        }

        public string Message { get; set; }
    }
    public static class DatsuGlobals
    {
        public static Theme SelectedTheme = Theme.Dark;
        public static bool IsBigEndian = false;
        public static event EventHandler<ThemeSwitchEventArgs> OnThemeSwitch;
        public static event EventHandler<StatusUpdateEventArgs> OnStatusUpdate;

        public static void ChangeTheme(Theme theme)
        {
            SelectedTheme = theme;
            OnThemeSwitch?.Invoke(null, new ThemeSwitchEventArgs(theme));
        }

        public static void UpdateStatus(string StatusMessage)
        {
            OnStatusUpdate?.Invoke(null, new StatusUpdateEventArgs(StatusMessage));
        }
    }
}
