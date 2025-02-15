using DatsuEditor.Common;
using System.Drawing;

namespace DatsuEditor.Nodes
{
    class HkxNode : FileNode
    {
        public HkxNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Havok Collision (*.hkx)|*.hkx";
            ReplaceFilter = "Havok Collision (*.hkx)|*.hkx";

            NodeIcon = DatsuResourceLoader.Icons["hkx.png"];
        }
    }
}
