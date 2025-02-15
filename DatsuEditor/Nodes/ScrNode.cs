using DatsuEditor.Common;

namespace DatsuEditor.Nodes
{
    class ScrNode : FileNode
    {
        public ScrNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Scene Resource (*.scr)|*.scr";
            ReplaceFilter = "Scene Resource (*.scr)|*.scr";

            NodeIcon = DatsuResourceLoader.Icons["scr.png"];
        }
    }
}
