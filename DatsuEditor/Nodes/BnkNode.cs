namespace DatsuEditor.Nodes
{
    class BnkNode : FileNode
    {
        public BnkNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Sound Bank (*.bnk)|*.bnk";
            ReplaceFilter = "Sound Bank (*.bnk)|*.bnk";

            ImageIndex = 7;
            SelectedImageIndex = 7;
        }
    }
}
