namespace ZanLibrary.Formats.Rendering.Effect
{
    public abstract class Record
    {
        public abstract string Id { get; set; }
        public byte[] BinaryData;

        public abstract void UpdateBinary();
        public abstract void ReadBinary(BinReader reader, EstData.TypeGroup type);
    }

    public class UnknownEffectStruct : Record
    {
        public UnknownEffectStruct(string Id, byte[] Data)
        {
            UnknownId = Id;
            BinaryData = Data;
        }
        private string UnknownId;
        public override string Id { get { return UnknownId; } set { } }
        public override void UpdateBinary()
        {

        }
        public override void ReadBinary(BinReader reader, EstData.TypeGroup type)
        {
            BinaryData = reader.ReadByteArray(type.Size);
        }
    }

    public class Part_s : Record
    {
        public override string Id { get { return "PART"; } set { } }

        public short u_a;
        public short u_b;
        public uint u_c;
        public uint u_d;
        public short[] u_e = new short[8]; // Assuming fixed size 8
        public uint[] u_f = new uint[9]; // Assuming fixed size 9

        public override void UpdateBinary()
        {
            BinWriter binWriter = new BinWriter();
            binWriter.WriteInt16(u_a);
            binWriter.WriteInt16(u_b);
            binWriter.WriteUInt32(u_c);
            binWriter.WriteUInt32(u_d);
            binWriter.WriteInt16Array(u_e, 8);
            binWriter.WriteUInt32Array(u_f, 9);
        }
        public override void ReadBinary(BinReader reader, EstData.TypeGroup type)
        {
            BinaryData = reader.ReadByteArray(type.Size);
        }
    }

    public class Move_s : Record
    {
        public override string Id { get { return "MOVE"; } set { } }

        public uint u_a;
        public float OffsetX;
        public float OffsetY;
        public float OffsetZ;
        public float[] u_b_1 = new float[15]; // Assuming fixed size 15
        public float angle;
        public float[] u_b_2 = new float[15]; // Assuming fixed size 15

        public float scale;

        public float[] u_c = new float[16]; // Assuming fixed size 16
        public float Red;
        public float Green;
        public float Blue;
        public float Alpha;
        public float[] u_d = new float[42]; // Assuming fixed size 42

        public override void UpdateBinary()
        {

        }
        public override void ReadBinary(BinReader reader, EstData.TypeGroup type)
        {
            BinaryData = reader.ReadByteArray(type.Size);
        }
    }

    public class Emif_s : Record
    {
        public override string Id { get { return "EMIF"; } set { } }

        public short count;
        public short u_a1;
        public short u_a2;
        public short u_a3;
        public short StartDelay;
        public short repeating;
        public float[] u_b = new float[9]; // Assuming fixed size 9
        public override void UpdateBinary()
        {

        }
        public override void ReadBinary(BinReader reader, EstData.TypeGroup type)
        {
            BinaryData = reader.ReadByteArray(type.Size);
        }
    }

    public class Tex_s : Record
    {
        public override string Id { get { return "TEX "; } set { } }

        public float u_a;
        public short u_b;
        public short u_c;
        public float[] u_d = new float[4]; // Assuming fixed size 4
        public Substruct[] substruct = new Substruct[2]; // Assuming fixed size 2

        public struct Substruct
        {
            public float u_d1;
            public short u_e;
            public byte[] u_f; // Assuming fixed size 2
            public float u_g;
            public byte[] u_h; // Assuming fixed size 4
            public float[] u_i; // Assuming fixed size 15

            public Substruct(int a)
            {
                u_d1 = 0.0f;
                u_e = 0;
                u_f = new byte[2];
                u_g = 0.0f;
                u_h = new byte[4];
                u_i = new float[15];
            }
        }

        public override void UpdateBinary()
        {

        }
        public override void ReadBinary(BinReader reader, EstData.TypeGroup type)
        {
            BinaryData = reader.ReadByteArray(type.Size);
        }
    }

    public class Pssa_s : Record
    {
        public override string Id { get { return "PSSA"; } set { } }

        public float[] u_a = new float[24]; // Assuming fixed size 24
        public override void UpdateBinary()
        {

        }
        public override void ReadBinary(BinReader reader, EstData.TypeGroup type)
        {
            BinaryData = reader.ReadByteArray(type.Size);
        }
    }

    public class Fvwk_s : Record
    {
        public override string Id { get { return "FVWK"; } set { } }

        public float[] u_a = new float[20]; // Assuming fixed size 20
        public override void UpdateBinary()
        {

        }
        public override void ReadBinary(BinReader reader, EstData.TypeGroup type)
        {
            BinaryData = reader.ReadByteArray(type.Size);
        }
    }

    public class Fwk_s : Record
    {
        public override string Id { get { return "FWK "; } set { } }

        public short unk_value;
        public short texture_num_1;
        public short texture_num_2;
        public short[] u_a = new short[3]; // Assuming fixed size 3
        public int[] u_c = new int[5]; // Assuming fixed size 5
        public override void UpdateBinary()
        {

        }
        public override void ReadBinary(BinReader reader, EstData.TypeGroup type)
        {
            BinaryData = reader.ReadByteArray(type.Size);
        }
    }

    public class Emmv_s : Record
    {
        public override string Id { get { return "EMMV"; } set { } }

        public uint u_a;

        public float left_pos1;
        public float top_pos;
        public float unk_pos1;
        public float random_pos1;
        public float top_bottom_random_pos1;
        public float front_back_random_pos1;
        public float left_pos2;
        public float front_pos1;
        public float front_pos2;
        public float left_right_random_pos1;
        public float random_pos2;
        public float front_back_random_pos2;
        public float unk_pos2;
        public float left_pos_random1;
        public float top_pos2;
        public float front_pos3;
        public float unk_pos3;
        public float unk_pos4;
        public float unk_pos5;
        public float unk_pos6; //19 (starts from 0)
        public float unk_pos7;
        public float unk_pos8;
        public float unk_pos9;

        public float unk_pos10;
        public float unk_pos11; //24
        public float unk_pos25;
        public float unk_pos26;
        public float unk_pos27;
        public float unk_pos28;
        public float unk_pos29;
        public float unk_pos30;
        public float unk_pos31;

        public float effect_size;
        public float[] u_b = new float[107]; // Assuming fixed size 107 - count of variables after u_a
        public override void UpdateBinary()
        {

        }
        public override void ReadBinary(BinReader reader, EstData.TypeGroup type)
        {
            BinaryData = reader.ReadByteArray(type.Size);
        }
    }

    public class Emfw_s : Record
    {
        public override string Id { get { return "EMFW"; } set { } }

        public int u_a;
        public short[] u_b = new short[2]; // Assuming fixed size 2
        public int[] u_c = new int[6]; // Assuming fixed size 6
        public override void UpdateBinary()
        {

        }
        public override void ReadBinary(BinReader reader, EstData.TypeGroup type)
        {
            BinaryData = reader.ReadByteArray(type.Size);
        }
    }

    public class Mjsg_s : Record
    {
        public override string Id { get { return "MJSG"; } set { } }

        public byte[] u_a = new byte[4]; // Assuming fixed size 4
        public float[] u_b = new float[4]; // Assuming fixed size 4
        public byte[] u_c = new byte[4]; // Assuming fixed size 4
        public int[] u_d = new int[2]; // Assuming fixed size 2
        public override void UpdateBinary()
        {

        }
        public override void ReadBinary(BinReader reader, EstData.TypeGroup type)
        {
            BinaryData = reader.ReadByteArray(type.Size);
        }
    }

    public class Mjcm_s : Record
    {
        public override string Id { get { return "MJCM"; } set { } }

        public short[] u_a; // Assuming fixed size 40
        public override void UpdateBinary()
        {

        }
        public override void ReadBinary(BinReader reader, EstData.TypeGroup type)
        {
            BinaryData = reader.ReadByteArray(type.Size);
        }
    }
}
