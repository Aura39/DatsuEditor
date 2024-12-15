using ZanLibrary.Formats.Rendering;

namespace DatsuEditor.Nodes
{
    class TextureNode : FileNode
    {
        uint TextureFlags;
        uint TextureIndex;
        public TextureNode(string filename, byte[] filedata, uint flags, uint index) : base(filename, filedata)
        {
            ExportFilter = "Texture (*.)|*.";
            ReplaceFilter = "Texture (*.)|*.";

            Text = index.ToString();
            TextureFlags = flags;
            TextureIndex = index;

            ImageIndex = 3;
            SelectedImageIndex = 3;
        }
    }
    class WtbNode : FileNode
    {
        public WtbNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Texture Set (*.wtb)|*.wtb";
            ReplaceFilter = "Texture Set (*.wtb)|*.wtb";

            ImageIndex = 3;
            SelectedImageIndex = 3;

            OnReplace += (e,s) => { UnpackBundle(); };

            UnpackBundle();
        }

        public void UnpackBundle()
        {
            Nodes.Clear();
            var bundleData = WtbFile.Load(Data);
            for (int i = 0; i < bundleData.Textures.Length; i++)
            {
                var texture = bundleData.Textures[i];
                var node = new TextureNode(texture.Index.ToString(), texture.Data, texture.Flags, texture.Index);
                Nodes.Add(node);
            }
        }
    }
}
