using DatsuEditor.Common;

namespace DatsuEditor.Nodes
{
    class WemNode : FileNode
    {
        public WemNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "WWise Encoded Media (*.wem)|*.wem";
            ReplaceFilter = "WWise Encoded Media (*.wem)|*.wem";

            NodeIcon = DatsuResourceLoader.Icons["wem.png"];
        }
    }
}
