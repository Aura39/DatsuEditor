using System;
using System.Collections.Generic;

namespace ZanLibrary
{
    public class BinReader
    {
        public Endianness Endian = Endianness.Little;
        byte[] _data;
        int _pointer = 0;
        public BinReader(byte[] data)
        {
            _data = data;
        }
        public BinReader(byte[] data, Endianness endian)
        {
            _data = data;
            Endian = endian;
        }
        public void Seek(uint offset)
        {
            _pointer = (int)offset;
        }

        // Single Value Reading
        public byte ReadByte()
        {
            return _data[_pointer++];
        }
        public uint ReadUInt32()
        {
            if (Endian == Endianness.Little)
            {
                _pointer += 4;
                return BitConverter.ToUInt32( _data, _pointer-4 );
            }
            else
            {
                byte[] bigendata = new byte[] { _data[_pointer + 3], _data[_pointer + 2], _data[_pointer + 1], _data[_pointer] };
                _pointer += 4;
                return BitConverter.ToUInt32(bigendata, 0);
            }
        }
        public int ReadInt32()
        {
            if (Endian == Endianness.Little)
            {
                _pointer += 4;
                return BitConverter.ToInt32(_data, _pointer - 4);
            }
            else
            {
                byte[] bigendata = new byte[] { _data[_pointer + 3], _data[_pointer + 2], _data[_pointer + 1], _data[_pointer] };
                _pointer += 4;
                return BitConverter.ToInt32(bigendata, 0);
            }
        }
        public float ReadFloat32()
        {
            if (Endian == Endianness.Little)
            {
                _pointer += 4;
                return BitConverter.ToSingle(_data, _pointer - 4);
            }
            else
            {
                byte[] bigendata = new byte[] { _data[_pointer + 3], _data[_pointer + 2], _data[_pointer + 1], _data[_pointer] };
                _pointer += 4;
                return BitConverter.ToSingle(bigendata, 0);
            }
        }
        public short ReadInt16()
        {
            if (Endian == Endianness.Little)
            {
                _pointer += 2;
                return BitConverter.ToInt16(_data, _pointer - 2);
            }
            else
            {
                byte[] bigendata = new byte[] { _data[_pointer + 1], _data[_pointer] };
                _pointer += 2;
                return BitConverter.ToInt16(bigendata, 0);
            }
        }
        public ushort ReadUInt16()
        {
            if (Endian == Endianness.Little)
            {
                _pointer += 2;
                return BitConverter.ToUInt16(_data, _pointer - 2);
            }
            else
            {
                byte[] bigendata = new byte[] { _data[_pointer + 1], _data[_pointer] };
                _pointer += 2;
                return BitConverter.ToUInt16(bigendata, 0);
            }
        }
        public string ReadString(int length)
        {
            string res = string.Empty;
            for (int i = 0; i < length; i++)
            {
                res += (char)_data[_pointer++];
            }
            return res;
        }
        public string ReadString()
        {
            string res = string.Empty;
            while (_data[_pointer] != '\0')
            {
                res += (char)_data[_pointer++];
            }
            return res;
        }
        // Arrays
        public string[] ReadStringArray(uint length, uint arrLen)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < arrLen; i++)
            {
                list.Add(ReadString((int)length));
            }
            return list.ToArray();
        }
        public uint[] ReadUInt32Array(uint length)
        {
            List<uint> list = new List<uint>();
            for (int i = 0; i < length; i++)
            {
                list.Add(ReadUInt32());
            }
            return list.ToArray();
        }
        public int[] ReadInt32Array(uint length)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < length; i++)
            {
                list.Add(ReadInt32());
            }
            return list.ToArray();
        }
        public byte[] ReadByteArray(uint length)
        {
            List<byte> list = new List<byte>();
            for (int i = 0; i < length; i++)
            {
                list.Add(ReadByte());
            }
            return list.ToArray();
        }
    }
}