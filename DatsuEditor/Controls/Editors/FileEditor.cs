using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DatsuEditor.Nodes;

namespace DatsuEditor.Controls.Editors
{
    public abstract class FileEditor : UserControl
    {
        public abstract bool LoadFile(FileNode Node);
        public abstract bool SaveFile();
    }
}
