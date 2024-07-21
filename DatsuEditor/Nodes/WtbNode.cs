namespace DatsuEditor.Nodes
{
    class WtbNode : FileNode
    {
        public WtbNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Texture Set (*.wtb)|*.wtb";
            ReplaceFilter = "Texture Set (*.wtb)|*.wtb";

            ImageIndex = 3;
            SelectedImageIndex = 3;
        }
    }
}
