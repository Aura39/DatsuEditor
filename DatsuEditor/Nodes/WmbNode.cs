using DatsuEditor.Common;

namespace DatsuEditor.Nodes
{
    class WmbNode : FileNode
    {
        public WmbNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Model (*.wmb)|*.wmb";
            ReplaceFilter = "Model (*.wmb)|*.wmb";

            NodeIcon = DatsuResourceLoader.Icons["wmb.png"];
        }
    }
}
