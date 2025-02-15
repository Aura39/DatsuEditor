using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ZanLibrary.Formats.Container;
using System.IO;
using System.Windows.Media.Imaging;

namespace DatsuEditor.Common
{
    internal class DatsuResourceLoader
    {
        public static Dictionary<string, BitmapImage> Icons = new(); 
        public DatsuResourceLoader()
        {

        }

        public static void ReloadIcons()
        {
            Icons.Clear();
            byte[] IconBytes = new byte[1];
            var IconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DatsuEditor.DatsuIcons.dat");
            IconBytes = new byte[IconStream.Length];
            IconStream.Read(IconBytes, 0, (int)IconStream.Length);
            var IconsDat = DatFile.Load(IconBytes);
            foreach (var icon in IconsDat.Files)
            {
                BitmapImage image = new();
                image.BeginInit();
                image.StreamSource = new MemoryStream(icon.Data);
                image.EndInit();
                Icons.TryAdd(icon.Name, image);
            }
            IconStream.Close();
        }
    }
}
