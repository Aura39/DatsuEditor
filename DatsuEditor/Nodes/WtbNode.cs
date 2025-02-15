using System;
using DatsuEditor.Common;
using ZanLibrary.Formats.Rendering;

namespace DatsuEditor.Nodes
{
    class TextureNode : FileNode
    {
        public uint TextureFlags;
        public uint TextureIndex;
        public TextureNode(string filename, byte[] filedata, uint flags, uint index) : base(filename, filedata)
        {
            ExportFilter = "Texture (*.)|*.";
            ReplaceFilter = "Texture (*.)|*.";

            Header = index.ToString();
            TextureFlags = flags;
            TextureIndex = index;

            NodeIcon = DatsuResourceLoader.Icons["dds.png"];

            OnReplace += RequestRegeneration;
            OnRemove += RequestRegeneration;
            OnRename += RequestRegeneration;
        }

        public void RequestRegeneration(object s, EventArgs e)
        {
            if (Parent is WtbNode)
            {
                ((WtbNode)Parent).UpdateBinary();
            }
        }
    }
    class WtbNode : FileNode
    {
        public WtbNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Texture Set (*.wtb)|*.wtb";
            ReplaceFilter = "Texture Set (*.wtb)|*.wtb";

            NodeIcon = DatsuResourceLoader.Icons["wtb.png"];

            OnReplace += (e,s) => { UnpackBundle(); };

            UnpackBundle();
        }

        public WtbBundle GetBundle()
        {
            WtbBundle bundle = new();
            bundle.HeaderSize = 0;
            bundle.IsMetadataOnly = false;
            bundle.Textures = new WtbTexture[Items.Count];
            int idx = 0;
            foreach (TextureNode node in Items)
            {
                WtbTexture tex = new();
                tex.Data = node.Data;
                tex.Flags = node.TextureFlags;
                tex.Index = node.TextureIndex;
                bundle.Textures[idx] = tex;
                idx++;
            }
            return bundle;
        }

        public void UpdateBinary()
        {
            Data = WtbFile.Save(GetBundle());
            if (Parent is CtxNode)
            {
                ((CtxNode)Parent).UpdateBinary();
            }
        }

        public void UnpackBundle()
        {
            Items.Clear();
            var bundleData = WtbFile.Load(Data);
            for (int i = 0; i < bundleData.Textures.Length; i++)
            {
                var texture = bundleData.Textures[i];
                var node = new TextureNode(texture.Index.ToString(), texture.Data, texture.Flags, texture.Index);
                Items.Add(node);
            }
        }
    }
}
