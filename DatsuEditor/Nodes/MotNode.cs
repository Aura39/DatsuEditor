namespace DatsuEditor.Nodes
{
    class MotNode : FileNode
    {
        public MotNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Motion (*.mot)|*.mot";
            ReplaceFilter = "Motion (*.mot)|*.mot";

            ImageIndex = 5;
            SelectedImageIndex = 5;
        }
    }
}
