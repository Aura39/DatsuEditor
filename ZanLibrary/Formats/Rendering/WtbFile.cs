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
        public int HeaderSize;
        public bool IsMetadataOnly;
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
            bundle.HeaderSize = 12 + (5 * 4) + ((int)header.NumTextures * 4 * 4);
            bundle.Textures = new WtbTexture[header.NumTextures];

            bundle.IsMetadataOnly = arrays.Sizes
                .Select((size) => { return size > data.Length; })
                .Contains(true);

            for (int i = 0; i < header.NumTextures; i++)
            {
                var tex = new WtbTexture();
                uint offset = arrays.Offsets[i];
                if (bundle.IsMetadataOnly)
                    tex.Data = new byte[1];
                else
                {
                    bin.Seek(offset);
                    tex.Data = bin.ReadByteArray(arrays.Sizes[i]);
                }
                tex.Flags = arrays.Flags[i];
                tex.Index = arrays.Indices[i];
                bundle.Textures[i] = tex;
            }

            return bundle;
        }

        public static byte[] Save(WtbBundle Bundle)
        {
            if (Bundle.IsMetadataOnly) // TODO: Make WTA writing separately
                return new byte[1];

            // TODO: Implement
            BinWriter bin = new();

            bin.WriteString("WTB\x00");
            bin.WriteUInt32(1);
            bin.WriteInt32(Bundle.Textures.Length);

            bin.WriteUInt32(0);
            bin.WriteUInt32(0);
            bin.WriteUInt32(0);
            bin.WriteUInt32(0);
            bin.WriteUInt32(0);

            int array_begin = bin.Tell();
            int array_size = Bundle.Textures.Length * 4;

            int data_begin = 32 + array_size * 4;
            for (int i = 0; i < array_size; i++)
            {
                bin.WriteInt32(0);
            }
            bin.Seek((uint)data_begin);

            List<int> offset_array = new();
            int offset_ptr = data_begin;
            foreach (var texture in Bundle.Textures)
            {
                offset_ptr += texture.Data.Length;
                offset_array.Add(offset_ptr);
                bin.WriteByteArray(texture.Data);
            }

            bin.Seek(32);

            foreach (int offset in offset_array)
                bin.ReplaceInt32(offset);
            foreach (var texture in Bundle.Textures)
                bin.ReplaceInt32(texture.Data.Length);
            foreach (var texture in Bundle.Textures)
                bin.ReplaceUInt32(texture.Flags);
            foreach (var texture in Bundle.Textures)
                bin.ReplaceUInt32(texture.Index);

            return bin.GetArray();
        }
    }
}
