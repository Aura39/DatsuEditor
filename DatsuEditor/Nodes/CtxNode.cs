using System;
using System.Collections.Generic;
using ZanLibrary.Formats.Container;
using ZanLibrary.Formats.Rendering;

namespace DatsuEditor.Nodes
{
    class CtxNode : FileNode
    {
        public event EventHandler OnBinaryRefresh;
        public CtxNode(string filename, byte[] filedata) : base(filename, filedata) {
            ExportFilter = "Cubemap Textures (*.ctx)|*.ctx";
            ReplaceFilter = "Cubemap Textures (*.ctx)|*.ctx";

            // TODO: Icon

            OnReplace += (e, s) => { UnpackCtx(); };

            UnpackCtx();
        }

        public void UpdateBinary()
        {
            List<byte[]> bundles = new List<byte[]>();
            foreach (FileNode node in Items)
            {
                if (node is WtbNode)
                {
                    bundles.Add(node.Data);
                }
            }
            CtxData ctx = new CtxData();
            ctx.Bundles = bundles;
            Data = СtxFile.Save(ctx);
            OnBinaryRefresh?.Invoke(this, new EventArgs());
        }
        public void UnpackCtx()
        {
            Items.Clear();
            var bundles = СtxFile.Load(Data);
            int idx = 0;
            foreach (var bundle in bundles.Bundles)
            {
                Items.Add(new WtbNode(Header.ToString() + $"_cube{idx:D2}", bundle));
                idx++;
            }
        }
    }
}
