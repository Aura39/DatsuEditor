using ZanLibrary.Formats.Container;

namespace DatsuEditor.Nodes
{
    class CtxNode : FileNode
    {
        public CtxNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Cubemap Textures (*.ctx)|*.ctx";
            ReplaceFilter = "Cubemap Textures (*.ctx)|*.ctx";

            ImageIndex = 14;
            SelectedImageIndex = 14;

            OnReplace += (e, s) => { UnpackCtx(); };

            UnpackCtx();
        }

        public void UnpackCtx()
        {
            Nodes.Clear();
            var bundles = СtxFile.Load(Data);
            int idx = 0;
            foreach (var bundle in bundles.Bundles)
            {
                Nodes.Add(new WtbNode(Text + $"_cube{idx:D2}", bundle));
                idx++;
            }
        }
    }
}
