using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZanLibrary.Formats.Rendering;

namespace ZanLibrary.Formats.Container
{
    public struct CtxHeader
    {
        public string Magic;
        public int Amount;
        public uint[] Offsets;
    }
    public struct CtxData
    {
        public List<byte[]> Bundles;
    }
    public struct CtxStruct
    {
        public CtxHeader Header;
        public CtxData Data;
    }
    public class СtxFile
    {
        public static CtxData Load(byte[] data)
        {
            CtxStruct ctx = new CtxStruct();

            BinReader bin = new BinReader(data);
            ctx.Header.Magic = bin.ReadString(4);
            ctx.Header.Amount = bin.ReadInt32();
            ctx.Header.Offsets = bin.ReadUInt32Array((uint)ctx.Header.Amount);

            bin.Seek(ctx.Header.Offsets[0]);
            ctx.Data = new CtxData();
            ctx.Data.Bundles = new List<byte[]>();
            for (int i = 0; i < ctx.Header.Amount; i++)
            {
                uint start = ctx.Header.Offsets[i == ctx.Header.Amount - 1 ? ctx.Header.Amount - 1 : i];
                uint end = i + 1 == ctx.Header.Amount ? (uint)data.Length : ctx.Header.Offsets[i + 1];
                uint len = end - start;
                byte[] wtbBytes = bin.ReadByteArray(len);
                ctx.Data.Bundles.Add(wtbBytes);
            }

            return ctx.Data;
        }

        public static byte[] Save(CtxData data)
        {
            BinWriter bin = new BinWriter();

            bin.WriteString("CT2\x00");
            bin.WriteUInt32((uint)data.Bundles.Count);

            List<int> offsets = new();
            int array_size = data.Bundles.Count * 4;
            int offset_ptr = 8 + array_size;
            for (int i = 0; i < data.Bundles.Count; i++)
                bin.WriteInt32(0);
            bin.Seek((uint)offset_ptr);
            foreach (var bundle in data.Bundles)
            {
                offsets.Add(offset_ptr);
                offset_ptr += bundle.Length;
                bin.WriteByteArray(bundle);
            }
            bin.Seek(8);
            bin.WriteInt32Array(offsets.ToArray());

            return bin.GetArray();
        }
    }
}
