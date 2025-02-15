using System.Windows.Documents;
using DatsuEditor.Common;
using ZanLibrary.Formats.Common;

namespace DatsuEditor.Nodes
{
    public class BxmNode : FileNode
    {
        public BxmNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Binary XML (*.bxm)|*.bxm";
            ReplaceFilter = "Binary XML (*.bxm)|*.bxm";

            NodeIcon = DatsuResourceLoader.Icons["bxm.png"];
        }
    }
}
