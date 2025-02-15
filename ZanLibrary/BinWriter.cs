using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZanLibrary
{
    class BinWriter
    {
        public Endianness Endian = Endianness.Little;
        List<byte> _data;
        int _pointer = 0;
        public BinWriter()
        {
            _data = new List<byte>();
        }
        public BinWriter(Endianness endian)
        {
            _data = new List<byte>();
            Endian = endian;
        }
        public BinWriter(byte[] data)
        {
            _data = data.ToList();
        }
        public BinWriter(byte[] data, Endianness endian)
        {
            _data = data.ToList();
            Endian = endian;
        }
        public void Seek(uint offset)
        {
            _pointer = (int)offset;
        }
        public int Tell()
        {
            return _pointer;
        }
        // Writing
        public void WriteByte(byte value)
        {
            _data.Insert(_pointer, value);
            _pointer += 1;
        }
        public void WriteUInt32(uint value)
        {
            if (Endian == Endianness.Little)
                _data.InsertRange(_pointer, BitConverter.GetBytes(value));
            else
                _data.InsertRange(_pointer, BitConverter.GetBytes(value).Reverse().ToArray());
            _pointer += 4;
        }
        public void WriteInt32(int value)
        {
            if (Endian == Endianness.Little)
                _data.InsertRange(_pointer, BitConverter.GetBytes(value));
            else
                _data.InsertRange(_pointer, BitConverter.GetBytes(value).Reverse().ToArray());
            _pointer += 4;
        }
        public void WriteFloat32(float value)
        {
            if (Endian == Endianness.Little)
                _data.InsertRange(_pointer, BitConverter.GetBytes(value));
            else
                _data.InsertRange(_pointer, BitConverter.GetBytes(value).Reverse().ToArray());
            _pointer += 4;
        }
        public void WriteInt16(short value)
        {
            if (Endian == Endianness.Little)
                _data.InsertRange(_pointer, BitConverter.GetBytes(value));
            else
                _data.InsertRange(_pointer, BitConverter.GetBytes(value).Reverse().ToArray());
            _pointer += 2;
        }
        public void WriteUInt16(ushort value)
        {
            if (Endian == Endianness.Little)
                _data.InsertRange(_pointer, BitConverter.GetBytes(value));
            else
                _data.InsertRange(_pointer, BitConverter.GetBytes(value).Reverse().ToArray());
            _pointer += 2;
        }
        public void WriteString(string value)
        {
            _data.InsertRange(_pointer, Encoding.ASCII.GetBytes(value));
            _pointer += value.Length;
        }
        public void WriteString(string value, int length)
        {
            int paddedLen = length - value.Length;
            _data.InsertRange(_pointer, Encoding.ASCII.GetBytes(value));
            _pointer += value.Length;
            for (int i = 0; i < paddedLen; i++)
            {
                WriteByte(0x00);
            }
        }
        public void WriteByteArray(byte[] value)
        {
            _data.InsertRange(_pointer, value);
            _pointer += value.Length;
        }
        public void WriteInt16Array(short[] value)
        {
            for (int i = 0; i < value.Length; i++)
                WriteInt16(value[i]);
            _pointer += value.Length * 2;
        }
        public void WriteInt16Array(short[] value, int length)
        {
            for (int i = 0; i < length; i++)
                WriteInt16(value[i]);
            _pointer += length * 2;
        }
        public void WriteInt32Array(int[] value)
        {
            for (int i = 0; i < value.Length; i++)
                WriteInt32(value[i]);
            _pointer += value.Length * 4;
        }
        public void WriteInt32Array(int[] value, int length)
        {
            for (int i = 0; i < length; i++)
                WriteInt32(value[i]);
            _pointer += length * 4;
        }
        public void WriteUInt32Array(uint[] value)
        {
            for (int i = 0; i < value.Length; i++)
                WriteUInt32(value[i]);
            _pointer += value.Length * 4;
        }
        public void WriteUInt32Array(uint[] value, int length)
        {
            for (int i = 0; i < length; i++)
                WriteUInt32(value[i]);
            _pointer += length * 4;
        }

        // Replacing
        public void ReplaceByte(byte value)
        {
            _data.RemoveAt(_pointer);
            _data.Insert(_pointer, value);
            _pointer += 1;
        }
        public void ReplaceUInt32(uint value)
        {
            _data.RemoveRange(_pointer, 4);
            WriteUInt32(value);
            _pointer += 4;
        }
        public void ReplaceInt32(int value)
        {
            _data.RemoveRange(_pointer, 4);
            WriteInt32(value);
            _pointer += 4;
        }
        public void ReplaceFloat32(float value)
        {
            _data.RemoveRange(_pointer, 4);
            WriteFloat32(value);
            _pointer += 4;
        }
        public void ReplaceInt16(short value)
        {
            _data.RemoveRange(_pointer, 2);
            WriteInt16(value);
            _pointer += 2;
        }
        public void ReplaceUInt16(ushort value)
        {
            _data.RemoveRange(_pointer, 2);
            WriteUInt16(value);
            _pointer += 2;
        }

        public void ReplaceRange(byte[] range)
        {
            _data.RemoveRange(_pointer, range.Length);
            _data.InsertRange(_pointer, range);
            _pointer += range.Length;
        }

        public void RemoveRange(int amount)
        {
            _data.RemoveRange(_pointer, amount);
        }
        public byte[] GetArray()
        {
            return _data.ToArray();
        }
        public int Size()
        {
            return _data.Count;
        }
    }
}
