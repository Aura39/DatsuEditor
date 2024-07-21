namespace DatsuEditor.Nodes
{
    class HkxNode : FileNode
    {
        public HkxNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Havok Collision (*.hkx)|*.hkx";
            ReplaceFilter = "Havok Collision (*.hkx)|*.hkx";

            ImageIndex = 18;
            SelectedImageIndex = 18;
        }
    }
}
