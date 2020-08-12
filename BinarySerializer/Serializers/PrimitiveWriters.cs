using System;
using BinarySerializer.Data;

namespace BinarySerializer.Serializers
{
    public interface IPrimitiveWriter<T>
    {
        int GetHashCode(T value);
        bool Equals(T a, T b);
        void WriteSize(BinaryDataWriter writer);
        void WriteData(BinaryDataWriter writer, T value);
        void SkipSize(BinaryDataReader reader);
        T ReadData(BinaryDataReader reader);
    }
    
    public sealed class BoolWriter : IPrimitiveWriter<bool>
    {
        public int GetHashCode(bool value)
        {
            return Convert.ToInt32(value);
        }

        public bool Equals(bool a, bool b)
        {
            return a == b;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.BoolSize);
        }

        public void WriteData(BinaryDataWriter writer, bool value)
        {
            writer.WriteBool(value);
        }

        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }

        public bool ReadData(BinaryDataReader reader)
        {
            return reader.ReadBool();
        }
    }

    public sealed class ByteWriter : IPrimitiveWriter<byte>
    {
        public int GetHashCode(byte value)
        {
            return value;
        }

        public bool Equals(byte a, byte b)
        {
            return a == b;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.ByteSize);
        }

        public void WriteData(BinaryDataWriter writer, byte value)
        {
            writer.WriteByte(value);
        }

        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }
        
        public byte ReadData(BinaryDataReader reader)
        {
            return reader.ReadByte();
        }
    }

    public sealed class CharWriter : IPrimitiveWriter<char>
    {
        public int GetHashCode(char value)
        {
            return value.GetHashCode();
        }

        public bool Equals(char a, char b)
        {
            return a == b;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.CharSize);
        }

        public void WriteData(BinaryDataWriter writer, char value)
        {
            writer.WriteChar(value);
        }
        
        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }

        public char ReadData(BinaryDataReader reader)
        {
            return reader.ReadChar();
        }
    }

    public sealed class DoubleWriter : IPrimitiveWriter<double>
    {
        public int GetHashCode(double value)
        {
            return value.GetHashCode();
        }

        public bool Equals(double a, double b)
        {
            return Math.Abs(a - b) < 1e-6;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.DoubleSize);
        }

        public void WriteData(BinaryDataWriter writer, double value)
        {
            writer.WriteDouble(value);
        }
        
        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }

        public double ReadData(BinaryDataReader reader)
        {
            return reader.ReadDouble();
        }
    }

    public sealed class FloatWriter : IPrimitiveWriter<float>
    {
        public int GetHashCode(float value)
        {
            return value.GetHashCode();
        }

        public bool Equals(float a, float b)
        {
            return Math.Abs(a - b) < 1e-6;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.FloatSize);
        }

        public void WriteData(BinaryDataWriter writer, float value)
        {
            writer.WriteFloat(value);
        }
        
        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }

        public float ReadData(BinaryDataReader reader)
        {
            return reader.ReadFloat();
        }
    }

    public sealed class IntWriter : IPrimitiveWriter<int>
    {
        public int GetHashCode(int value)
        {
            return value;
        }

        public bool Equals(int a, int b)
        {
            return a == b;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.IntSize);
        }

        public void WriteData(BinaryDataWriter writer, int value)
        {
            writer.WriteInt(value);
        }
        
        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }
        
        public int ReadData(BinaryDataReader reader)
        {
            return reader.ReadInt();
        }
    }

    public sealed class LongWriter : IPrimitiveWriter<long>
    {
        public int GetHashCode(long value)
        {
            return value.GetHashCode();
        }

        public bool Equals(long a, long b)
        {
            return a == b;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.LongSize);
        }

        public void WriteData(BinaryDataWriter writer, long value)
        {
            writer.WriteLong(value);
        }
        
        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }

        public long ReadData(BinaryDataReader reader)
        {
            return reader.ReadLong();
        }
    }

    public sealed class SByteWriter : IPrimitiveWriter<sbyte>
    {
        public int GetHashCode(sbyte value)
        {
            return value;
        }

        public bool Equals(sbyte a, sbyte b)
        {
            return a == b;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.SByteSize);
        }

        public void WriteData(BinaryDataWriter writer, sbyte value)
        {
            writer.WriteSByte(value);
        }
        
        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }

        public sbyte ReadData(BinaryDataReader reader)
        {
            return reader.ReadSByte();
        }
    }

    public sealed class ShortWriter : IPrimitiveWriter<short>
    {
        public int GetHashCode(short value)
        {
            return value;
        }

        public bool Equals(short a, short b)
        {
            return a == b;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.ShortSize);
        }

        public void WriteData(BinaryDataWriter writer, short value)
        {
            writer.WriteShort(value);
        }
        
        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }
        
        public short ReadData(BinaryDataReader reader)
        {
            return reader.ReadShort();
        }
    }

    public sealed class ShortFloatWriter : IPrimitiveWriter<float>
    {
        public int GetHashCode(float value)
        {
            return value.GetHashCode();
        }

        public bool Equals(float a, float b)
        {
            return Math.Abs(a - b) < 1e-6;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.ShortSize);
        }

        public void WriteData(BinaryDataWriter writer, float value)
        {
            writer.WriteShortFloat(value);
        }
        
        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }

        public float ReadData(BinaryDataReader reader)
        {
            return reader.ReadShortFloat();
        }
    }

    public sealed class StringWriter : IPrimitiveWriter<string>
    {
        public int GetHashCode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < value.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ value[i];
                    if (i == value.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ value[i + 1];
                }

                return hash1 + hash2 * 1566083941;
            }
        }

        public bool Equals(string a, string b)
        {
            return a == b;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
        }
        
        public void SkipSize(BinaryDataReader reader)
        {
        }

        public void WriteData(BinaryDataWriter writer, string value)
        {
            writer.WriteString(value);
        }

        public string ReadData(BinaryDataReader reader)
        {
            return reader.ReadString();
        }
    }

    public sealed class UIntWriter : IPrimitiveWriter<uint>
    {
        public int GetHashCode(uint value)
        {
            return value.GetHashCode();
        }

        public bool Equals(uint a, uint b)
        {
            return a == b;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.UIntSize);
        }

        public void WriteData(BinaryDataWriter writer, uint value)
        {
            writer.WriteUInt(value);
        }
        
        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }

        public uint ReadData(BinaryDataReader reader)
        {
            return reader.ReadUInt();
        }
    }

    public sealed class ULongWriter : IPrimitiveWriter<ulong>
    {
        public int GetHashCode(ulong value)
        {
            return value.GetHashCode();
        }

        public bool Equals(ulong a, ulong b)
        {
            return a == b;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.ULongSize);
        }

        public void WriteData(BinaryDataWriter writer, ulong value)
        {
            writer.WriteULong(value);
        }
        
        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }

        public ulong ReadData(BinaryDataReader reader)
        {
            return reader.ReadULong();
        }
    }

    public sealed class UShortWriter : IPrimitiveWriter<ushort>
    {
        public int GetHashCode(ushort value)
        {
            return value.GetHashCode();
        }
        
        public bool Equals(ushort a, ushort b)
        {
            return a == b;
        }

        public void WriteSize(BinaryDataWriter writer)
        {
            writer.WriteUShort(PrimitiveSize.UShortSize);
        }

        public void WriteData(BinaryDataWriter writer, ushort value)
        {
            writer.WriteUShort(value);
        }
        
        public void SkipSize(BinaryDataReader reader)
        {
            reader.ReadUShort();
        }

        public ushort ReadData(BinaryDataReader reader)
        {
            return reader.ReadUShort();
        }
    }
}