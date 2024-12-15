using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZanLibrary.Formats.Rendering
{
    public struct WtbHeader
    {
        public string Magic;
        public uint Unk04;
        public uint NumTextures;

        public uint fOffsetArray;
        public uint fSizeArray;
        public uint fFlagArray;
        public uint fIndexArray;
        public uint fInfoArray;
    }

    public struct WtbArrays
    {
        public uint[] Offsets;
        public uint[] Sizes;
        public uint[] Flags;
        public uint[] Indices;
    }

    public struct WtbTexture
    {
        public uint Index;
        public uint Flags;
        public byte[] Data;
    }

    public struct WtbBundle
    {
        public WtbTexture[] Textures;
    }

    public class WtbFile
    {
        public static WtbBundle Load(byte[] data)
        {
            WtbHeader header = new WtbHeader();
            WtbArrays arrays = new WtbArrays();

            BinReader bin = new BinReader(data);
            header.Magic = bin.ReadString(4);
            header.Unk04 = bin.ReadUInt32();
            header.NumTextures = bin.ReadUInt32();

            header.fOffsetArray = bin.ReadUInt32();
            header.fSizeArray = bin.ReadUInt32();
            header.fFlagArray = bin.ReadUInt32();
            header.fIndexArray = bin.ReadUInt32();
            header.fInfoArray = bin.ReadUInt32();

            bin.Seek(header.fOffsetArray);
            arrays.Offsets = bin.ReadUInt32Array(header.NumTextures);
            
            bin.Seek(header.fSizeArray);
            arrays.Sizes = bin.ReadUInt32Array(header.NumTextures);
            
            bin.Seek(header.fFlagArray);
            arrays.Flags = bin.ReadUInt32Array(header.NumTextures);
            
            bin.Seek(header.fIndexArray);
            arrays.Indices = bin.ReadUInt32Array(header.NumTextures);

            WtbBundle bundle = new WtbBundle();
            bundle.Textures = new WtbTexture[header.NumTextures];
            for (int i = 0; i < header.NumTextures; i++)
            {
                var tex = new WtbTexture();
                uint offset = arrays.Offsets[i];
                bin.Seek(offset);
                tex.Data = bin.ReadByteArray(arrays.Sizes[i]);
                tex.Flags = arrays.Flags[i];
                tex.Index = arrays.Indices[i];
                bundle.Textures[i] = tex;
            }

            return bundle;
        }
    }
}
