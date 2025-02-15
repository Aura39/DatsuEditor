using DatsuEditor.Common;

namespace DatsuEditor.Nodes
{
    class EstNode : FileNode
    {
        public EstNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Effect Structure (*.est)|*.est";
            ReplaceFilter = "Effect Structure (*.est)|*.est";

            NodeIcon = DatsuResourceLoader.Icons["est.png"];

        }
    }
}
