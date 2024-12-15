using System.Collections.Generic;

namespace ZanLibrary.Formats.Rendering.Effect
{
    public struct EstDataEntry
    {
        public EstData.TypeGroup[,] Types;
        public Record[] Records;
    }
    public class EstFile
    {
        public static EstDataEntry Load(byte[] data)
        {
            EstData.EffectHeader header = new EstData.EffectHeader();
            BinReader reader = new BinReader(data);

            header.Magic = reader.ReadString(4);
            header.RecordCount = reader.ReadUInt32();
            header.RecordOffsetsOffset = reader.ReadUInt32();
            header.TypeOffset = reader.ReadUInt32();
            header.TypeEndOffset = reader.ReadUInt32();
            header.TypeSize = reader.ReadUInt32();
            header.TypeNumber = reader.ReadUInt32();

            // Reading all record offsets
            reader.Seek(header.RecordOffsetsOffset);
            var Offsets = new uint[header.RecordCount];
            for (int i = 0; i < header.RecordCount; i++)
                Offsets[i] = reader.ReadUInt32();

            // Reading all types
            var Types = new EstData.TypeGroup[header.RecordCount, header.TypeNumber];
            reader.Seek(header.TypeOffset);
            for (int i = 0; i < header.RecordCount; i++)
            {
                for (int j = 0; j < header.TypeNumber; j++)
                    Types[i, j] = new EstData.TypeGroup
                    {
                        Padding = reader.ReadUInt32(),
                        Id = reader.ReadString(4),
                        Size = reader.ReadUInt32(),
                        Offset = reader.ReadUInt32()
                    };
            }

            List<Record> Records = new List<Record>();
            for (int i = 0; i < header.RecordCount; i++)
            {
                reader.Seek(Offsets[i]);
                for (int j = 0; j < header.TypeNumber; j++)
                {
                    if (Types[i, j].Size == 0)
                        continue;
                    reader.Seek(Offsets[i] + Types[i, j].Offset);
                    switch (Types[i, j].Id)
                    {
                        case "PART":
                            Records.Add(new Part_s());
                            break;
                        case "MOVE":
                            Records.Add(new Move_s());
                            break;
                        case "EMIF":
                            Records.Add(new Emif_s());
                            break;
                        case "TEX ":
                            Records.Add(new Tex_s());
                            break;
                        case "PSSA":
                            Records.Add(new Pssa_s());
                            break;
                        case "FVWK":
                            Records.Add(new Fvwk_s());
                            break;
                        case "FWK ":
                            Records.Add(new Fwk_s());
                            break;
                        case "EMMV":
                            Records.Add(new Emmv_s());
                            break;
                        case "EMFW":
                            Records.Add(new Emfw_s());
                            break;
                        case "MJSG":
                            Records.Add(new Mjsg_s());
                            break;
                        case "MJCM":
                            Records.Add(new Mjcm_s());
                            break;
                        default:
                            Records.Add(new UnknownEffectStruct(Types[i, j].Id, new byte[0]));
                            break;
                    }
                }
            }

            return new EstDataEntry
            {
                Types = Types,
                Records = Records.ToArray(),
            };
        }
    }
}

