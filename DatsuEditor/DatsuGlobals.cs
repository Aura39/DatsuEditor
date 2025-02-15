using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DatsuEditor.Nodes;

namespace DatsuEditor
{
    public enum DatsuThemes
    {
        Light,
        Dark,
    }
    public class ThemeChangeEventArgs : EventArgs
    {
        public DatsuThemes Theme;
    }
    public class StatusEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
    public class EditorRequestEventArgs : EventArgs
    {
        public FileNode TargetFile;
    }
    public class ChangePropertyObjectEventArgs : EventArgs
    {
        public object Target;
    }
    internal class DatsuEvents
    {
        public static event EventHandler<StatusEventArgs> OnStatusSubmit;
        public static event EventHandler<ThemeChangeEventArgs> OnThemeChange;
        public static event EventHandler<EditorRequestEventArgs> OnEditorRequest;
        public static event EventHandler<ChangePropertyObjectEventArgs> OnRequestPropertyEditor;

        public static void GlobalException(object s, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Runtime Exception!\n{e.ExceptionObject.ToString()}", "DatsuEditor", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void RequestPropertyEditor(object Target)
        {
            OnRequestPropertyEditor?.Invoke(null, new ChangePropertyObjectEventArgs() { Target = Target });
        }

        public static void SubmitStatus(string message)
        {
            OnStatusSubmit?.Invoke(null, new StatusEventArgs() { Message = message });
        }
        public static void RequestEditor(FileNode ForFile)
        {
            OnEditorRequest?.Invoke(null, new EditorRequestEventArgs() { TargetFile = ForFile });
        }

        public static void ChangeTheme(DatsuThemes theme)
        {
            OnThemeChange?.Invoke(null, new ThemeChangeEventArgs() { Theme = theme });
        }
    }
}
