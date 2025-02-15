using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using ZanLibrary.Hashes;

namespace ZanLibrary.Formats.Container
{
    struct DatHeader
    {
        public string Magic;
        public uint FileAmount;
        public uint PositionsOffset;
        public uint ExtensionsOffset;
        public uint NamesOffset;
        public uint SizesOffset;
        public uint HashMapOffset;
    }
    public struct DatFileEntry
    {
        public string Name;
        public byte[] Data;

        public DatFileEntry(string Name, byte[] Data)
        {
            this.Name = Name;
            this.Data = Data;
        }
    }
    public struct DatHashData
    {
        public int PrehashShift;
        public List<short> BucketOffsets;
        public List<int> Hashes;
        public List<short> Indices;
        public int StructSize;
    }

    public struct DatLoadResult
    {
        public Endianness Endian;
        public DatFileEntry[] Files;
    }

    public class DatFile
    {
        public static DatLoadResult Load(byte[] data, bool IsBigEndian = false)
        {
            StringBuilder log = new StringBuilder();
            log.AppendLine("Load DAT: Start");
            uint nameSize = 1;
            DatHeader header = new DatHeader();

            DatLoadResult Return = new DatLoadResult();
            List<DatFileEntry> Files = new List<DatFileEntry>();

            Return.Endian = Endianness.Little;
            BinReader reader = new BinReader(data, Endianness.Little);
            header.Magic = reader.ReadString(4);
            header.FileAmount = reader.ReadUInt32();
            header.PositionsOffset = reader.ReadUInt32();
            if (header.PositionsOffset == 536870912)
            {
                reader.Endian = Endianness.Big;
                Return.Endian = Endianness.Big;
                reader.Seek(0);
                header.Magic = reader.ReadString(4);
                header.FileAmount = reader.ReadUInt32();
                header.PositionsOffset = reader.ReadUInt32();
            }
            header.ExtensionsOffset = reader.ReadUInt32();
            header.NamesOffset = reader.ReadUInt32();
            header.SizesOffset = reader.ReadUInt32();
            header.HashMapOffset = reader.ReadUInt32();

            reader.Seek(header.PositionsOffset);
            int[] fileOffsets = reader.ReadInt32Array(header.FileAmount);

            reader.Seek(header.SizesOffset);
            int[] fileSizes = reader.ReadInt32Array(header.FileAmount);

            reader.Seek(header.NamesOffset);
            nameSize = reader.ReadUInt32();
            string[] fileNames = reader.ReadStringArray(nameSize, header.FileAmount);

            for (int i = 0; i < header.FileAmount; i++)
            {
#if DEBUG
                Stopwatch watch = new Stopwatch();
                watch.Start();
#endif
                int filePosition = fileOffsets[i];
                int fileSize = fileSizes[i];
                string fileName = fileNames[i];

                byte[] fileData = new byte[fileSize];
                for (int j = 0; j < fileSize; j++)
                {
                    fileData[j] = data[filePosition + j];
                }
                Files.Add(new DatFileEntry(fileName.Replace("\0", string.Empty), fileData));
#if DEBUG
                watch.Stop();
                log.AppendLine($"Load DAT: {fileName.Trim()} read in {watch.ElapsedTicks} ticks.");
#endif
                // MessageBox.Show(i.ToString());
            }
#if DEBUG
            File.WriteAllText("DatFile-Load.log", log.ToString());
#endif
            Return.Files = Files.ToArray();
            return Return;
        }

        public static byte[] Save(DatLoadResult Data)
        {
            /*
             * Struct generation phase
             */

            DatHeader header = new DatHeader();

            header.FileAmount = (uint)Data.Files.Length;

            int LongestFilename = 0;

            List<int> offsets = new List<int>();
            List<int> sizes = new List<int>();
            List<string> extensions = new List<string>();
            List<string> names = new List<string>();

            foreach (DatFileEntry file in Data.Files)
            {
                if (LongestFilename < file.Name.Length)
                    LongestFilename = file.Name.Length;

                sizes.Add(file.Data.Length);
                extensions.Add(Path.GetExtension(file.Name).Substring(1));
                names.Add(file.Name);
            }

            LongestFilename += 1;

            header.PositionsOffset = 0x20;
            header.ExtensionsOffset = 0x20 + 4 * header.FileAmount;
            header.NamesOffset = header.ExtensionsOffset + 4 * header.FileAmount;
            header.SizesOffset = (uint)(header.NamesOffset + LongestFilename * header.FileAmount + 4) + 2;
            header.HashMapOffset = header.SizesOffset + 4 * header.FileAmount;

            DatHashData hashData = DatHashUtil.GenerateHashData(Data.Files);

            int TempPos = (int)(header.HashMapOffset + hashData.StructSize + 2 * header.FileAmount);
            int startpad = BinUtils.CalcPadding(16, TempPos);

            int _pointer = TempPos + startpad;

            foreach (var file in Data.Files)
            {
                offsets.Add(_pointer);
                _pointer += file.Data.Length;
                uint pad = BinUtils.CalcPadding(16, (uint)_pointer);
                _pointer += (int)pad;
            }

            /*
             * Byte generation phase
             */
            BinWriter FileData = new BinWriter();
            FileData.Endian = Data.Endian;

            FileData.WriteString("DAT\x00"); // DAT\x00
            FileData.WriteUInt32(header.FileAmount);
            FileData.WriteUInt32(header.PositionsOffset);
            FileData.WriteUInt32(header.ExtensionsOffset);
            FileData.WriteUInt32(header.NamesOffset);
            FileData.WriteUInt32(header.SizesOffset);
            FileData.WriteUInt32(header.HashMapOffset);
            FileData.WriteUInt32(0);

            FileData.Seek(header.PositionsOffset);
            for (int i = 0; i < header.FileAmount; i++)
                FileData.WriteUInt32((uint)offsets[i]);

            FileData.Seek(header.ExtensionsOffset);
            for (int i = 0; i < header.FileAmount; i++)
            {
                FileData.WriteString(extensions[i]);
                FileData.WriteByte(0x00);
            }

            FileData.Seek(header.NamesOffset);
            FileData.WriteInt32(LongestFilename);

            for (int i = 0; i < header.FileAmount; i++)
            {
                FileData.WriteString(names[i], LongestFilename);
                // FileData.WriteByte(0x00);
            }

            FileData.WriteInt16(0x00);

            FileData.Seek(header.SizesOffset);
            for (int i = 0; i < header.FileAmount; i++)
                FileData.WriteUInt32((uint)sizes[i]);

            /*
             * Hashes :sob::sob::sob::sob::sob:
             */
            FileData.Seek(header.HashMapOffset);
            FileData.WriteInt32(hashData.PrehashShift);
            FileData.WriteInt32(16);
            FileData.WriteInt32(16 + hashData.BucketOffsets.Count * 2);
            FileData.WriteInt32(16 + hashData.BucketOffsets.Count * 2 + hashData.Hashes.Count * 4);

            for (int i = 0; i < hashData.BucketOffsets.Count; i++)
                FileData.WriteInt16(hashData.BucketOffsets[i]);

            for (int i = 0; i < header.FileAmount; i++)
                FileData.WriteInt32(hashData.Hashes[i]);

            for (int i = 0; i < header.FileAmount; i++)
                FileData.WriteInt16(hashData.Indices[i]);

            int hashPad = (int)BinUtils.CalcPadding(16, (uint)FileData.Tell());
            for (int i = 0; i < hashPad; i++)
                FileData.WriteByte(0x00);

            for (int i = 0; i < header.FileAmount; i++)
            {
                if (offsets[i] > FileData.Size())
                {
                    int extend = offsets[i] - FileData.Size();
                    for (int j = 0; j < extend; j++)
                    {
                        FileData.WriteByte(0x00);
                    }
                }
                FileData.Seek((uint)offsets[i]);
                FileData.WriteByteArray(Data.Files[i].Data);
            }

            //for (int i = 0; i < names.Count; i++)
            //{
            //    uint search = (Crc32.HashToUInt32(Encoding.ASCII.GetBytes(names[i].ToLower())) & ~0x80000000) >> hashData.PrehashShift;
            //    Console.WriteLine($"Exp: {names[i]}\tOut: {names[hashData.Indices[hashData.BucketOffsets[(int)search]]]}");
            //}

            return FileData.GetArray();
        }
    }
    public class DatHashUtil
    {
        class HashStruct
        {
            public int Hash;
            public short Index;

            public override string ToString()
            {
                return $"({Hash:X}, {Index:X})";
            }
        };

        private static HashStruct MakeHashStruct(DatFileEntry t, int i)
        {
            return new HashStruct
            {
                Hash = (int)Crc32.HashToUInt32(Encoding.ASCII.GetBytes(t.Name.ToLower())) & 0x7FFFFFFF,
                Index = (short)i
            };
        }

        // https://github.com/Petrarca181/MAMMT/blob/master/MAMMT/Workers/0_Dat_Repacktor.cs
        public static DatHashData GenerateHashData(DatFileEntry[] Files)
        {
            var FileAmountLength = (int)Math.Log(Files.Length, 2) + 1;
            var PrehashShift = Math.Min(31, 32 - FileAmountLength);
            var BucketSize = 1 << 31 - PrehashShift;
            var BucketOffsets = Enumerable.Repeat((short)-1, BucketSize).ToList();

            var hashTuple = Files.Select((t, i) => MakeHashStruct(t, i)).OrderBy(x => x.Hash >> PrehashShift).ToList();

            for (var i = 0; i < Files.Length; i++)
                if (BucketOffsets[hashTuple[i].Hash >> PrehashShift] == -1)
                    BucketOffsets[hashTuple[i].Hash >> PrehashShift] = (short)i;

            return new DatHashData
            {
                PrehashShift = PrehashShift,
                BucketOffsets = BucketOffsets,
                Hashes = hashTuple.Select(x => x.Hash).ToList(),
                Indices = hashTuple.Select(x => x.Index).ToList(),
                StructSize = 4 + 2 * BucketOffsets.Count + 4 * hashTuple.Count() + 2 * hashTuple.Count()
            };
        }
    }
}
