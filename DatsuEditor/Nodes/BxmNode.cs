namespace DatsuEditor.Nodes
{
    class BxmNode : FileNode
    {
        public BxmNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Binary XML (*.bxm)|*.bxm";
            ReplaceFilter = "Binary XML (*.bxm)|*.bxm";

            ImageIndex = 2;
            SelectedImageIndex = 2;
        }
    }
}
