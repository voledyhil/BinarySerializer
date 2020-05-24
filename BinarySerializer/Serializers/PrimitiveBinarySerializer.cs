using System;
using System.Reflection;
using BinarySerializer.Data;
using BinarySerializer.Expressions;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer.Serializers
{
    public abstract class PrimitiveBinarySerializer<T> : IBinarySerializer<byte>
    {
        protected readonly byte Index;
        protected readonly Getter<T> Getter;
        protected readonly Setter<T> Setter;

        protected PrimitiveBinarySerializer(byte index, Type ownerType, FieldInfo field)
        {
            Index = index;
            Getter = new Getter<T>(ownerType, field);
            Setter = new Setter<T>(ownerType, field);
        }

        void IBinarySerializer.Serialize(object obj, BinaryDataWriter writer, IBaseline baseline)
        {
            Serialize(obj, writer, (IBaseline<byte>) baseline);
        }

        public abstract void Update(object obj, BinaryDataReader reader);
        public abstract void Serialize(object obj, BinaryDataWriter writer);
        public abstract void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline);
        
    }

    public sealed class BoolBinarySerializer : PrimitiveBinarySerializer<bool>
    {
        public BoolBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static int GetHashCode(bool value)
        {
            return Convert.ToInt32(value);
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadBool());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            bool value = Getter.Get(obj);
            if (value == default)
                return;

            writer.WriteByte(Index);
            writer.WriteBool(true);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            bool value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && value == default || baseHash == hash)
                return;
            baseline[Index] = hash;
            writer.WriteByte(Index);
            writer.WriteBool(value);
        }
    }

    public sealed class ByteBinarySerializer : PrimitiveBinarySerializer<byte>
    {
        public ByteBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static int GetHashCode(byte value)
        {
            return Convert.ToInt32(value);
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadByte());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            byte value = Getter.Get(obj);
            if (value == default)
                return;

            writer.WriteByte(Index);
            writer.WriteByte(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            byte value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && value == default || baseHash == hash)
                return;
            baseline[Index] = hash;
            writer.WriteByte(Index);
            writer.WriteByte(value);
        }
    }

    public sealed class CharBinarySerializer : PrimitiveBinarySerializer<char>
    {
        public CharBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static int GetHashCode(char value)
        {
            return Convert.ToInt32(value);
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadChar());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            char value = Getter.Get(obj);
            if (value == default)
                return;

            writer.WriteByte(Index);
            writer.WriteChar(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            char value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && value == default || baseHash == hash)
                return;
            baseline[Index] = hash;
            writer.WriteByte(Index);
            writer.WriteChar(value);
        }
    }

    public sealed class DoubleBinarySerializer : PrimitiveBinarySerializer<double>
    {
        public DoubleBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static unsafe int GetHashCode(double value)
        {
            double d = value; 
            if (Math.Abs(d) < 1e-6)
                return 0;
            long l = *(long*)&d;
            return unchecked((int)l) ^ (int)(l >> 32); 
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadDouble());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            double value = Getter.Get(obj);
            if (Math.Abs(value) < 1e-6)
                return;

            writer.WriteByte(Index);
            writer.WriteDouble(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            double value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && Math.Abs(value) < 1e-6 || baseHash == hash)
                return;
            baseline[Index] = hash;
            
            writer.WriteByte(Index);
            writer.WriteDouble(value);
        }
    }

    public sealed class FloatBinarySerializer : PrimitiveBinarySerializer<float>
    {
        public FloatBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static unsafe int GetHashCode(float value)
        {
            return Math.Abs(value) < 1e-6 ? 0 : *(int*) &value;
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadFloat());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            float value = Getter.Get(obj);
            if (Math.Abs(value) < 1e-6)
                return;

            writer.WriteByte(Index);
            writer.WriteFloat(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            float value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && Math.Abs(value) < 1e-6 || baseHash == hash)
                return;
            baseline[Index] = hash;
            writer.WriteByte(Index);
            writer.WriteFloat(value);
        }
    }

    public sealed class IntBinarySerializer : PrimitiveBinarySerializer<int>
    {
        public IntBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static int GetHashCode(int value)
        {
            return value;
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadInt());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            int value = Getter.Get(obj);
            if (value == default)
                return;

            writer.WriteByte(Index);
            writer.WriteInt(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            int value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && value == default || baseHash == hash)
                return;
            baseline[Index] = hash;
            writer.WriteByte(Index);
            writer.WriteInt(value);
        }
    }

    public sealed class LongBinarySerializer : PrimitiveBinarySerializer<long>
    {
        public LongBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static int GetHashCode(long value)
        {
            return unchecked((int) value);
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadLong());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            long value = Getter.Get(obj);
            if (value == default)
                return;

            writer.WriteByte(Index);
            writer.WriteLong(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            long value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && value == default || baseHash == hash)
                return;
            baseline[Index] = hash;
            writer.WriteByte(Index);
            writer.WriteLong(value);
        }
    }

    public sealed class SByteBinarySerializer : PrimitiveBinarySerializer<sbyte>
    {
        public SByteBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static int GetHashCode(sbyte value)
        {
            return Convert.ToInt32(value);
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadSByte());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            sbyte value = Getter.Get(obj);
            if (value == default)
                return;

            writer.WriteByte(Index);
            writer.WriteSByte(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            sbyte value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && value == default || baseHash == hash)
                return;
            baseline[Index] = hash;
            writer.WriteByte(Index);
            writer.WriteSByte(value);
        }
    }

    public sealed class ShortBinarySerializer : PrimitiveBinarySerializer<short>
    {
        public ShortBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static int GetHashCode(short value)
        {
            return Convert.ToInt32(value);
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadShort());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            short value = Getter.Get(obj);
            if (value == default)
                return;

            writer.WriteByte(Index);
            writer.WriteShort(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            short value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && value == default || baseHash == hash)
                return;
            baseline[Index] = hash;
            writer.WriteByte(Index);
            writer.WriteShort(value);
        }
    }

    public sealed class ShortFloatBinarySerializer : PrimitiveBinarySerializer<float>
    {
        public ShortFloatBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static unsafe int GetHashCode(float value)
        {
            return Math.Abs(value) < 1e-6 ? 0 : *(int*) &value;
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadShortFloat());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            float value = Getter.Get(obj);
            if (Math.Abs(value) < 1e-6)
                return;

            writer.WriteByte(Index);
            writer.WriteShortFloat(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            float value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && Math.Abs(value) < 1e-6 || baseHash == hash)
                return;
            baseline[Index] = hash;
            writer.WriteByte(Index);
            writer.WriteShortFloat(value);
        }
    }

    public sealed class StringBinarySerializer : PrimitiveBinarySerializer<string>
    {
        public StringBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static int GetHashCode(string value)
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

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadString());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            string value = Getter.Get(obj);
            if (string.IsNullOrEmpty(value))
                return;

            writer.WriteByte(Index);
            writer.WriteString(Getter.Get(obj));
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            string value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && string.IsNullOrEmpty(value) || baseHash == hash)
                return;
            baseline[Index] = hash;
            
            writer.WriteByte(Index);
            writer.WriteString(value);
        }
    }

    public sealed class UIntBinarySerializer : PrimitiveBinarySerializer<uint>
    {
        public UIntBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static int GetHashCode(uint value)
        {
            return unchecked((int) value);
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadUInt());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            uint value = Getter.Get(obj);
            if (value == default)
                return;

            writer.WriteByte(Index);
            writer.WriteUInt(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            uint value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && value == default || baseHash == hash)
                return;
            baseline[Index] = hash;
            writer.WriteByte(Index);
            writer.WriteUInt(value);
        }
    }

    public sealed class ULongBinarySerializer : PrimitiveBinarySerializer<ulong>
    {
        public ULongBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static int GetHashCode(ulong value)
        {
            return unchecked((int) value);
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadULong());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            ulong value = Getter.Get(obj);
            if (value == default)
                return;

            writer.WriteByte(Index);
            writer.WriteULong(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            ulong value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && value == default || baseHash == hash)
                return;
            baseline[Index] = hash;
            writer.WriteByte(Index);
            writer.WriteULong(value);
        }
    }

    public sealed class UShortBinarySerializer : PrimitiveBinarySerializer<ushort>
    {
        public UShortBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        private static int GetHashCode(ushort value)
        {
            return Convert.ToInt32(value);
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, reader.ReadUShort());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            ushort value = Getter.Get(obj);
            if (value == default)
                return;

            writer.WriteByte(Index);
            writer.WriteUShort(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            ushort value = Getter.Get(obj);
            int hash = GetHashCode(value);
            int baseHash = baseline[Index];
            if (baseHash == 0 && value == default || baseHash == hash)
                return;
            baseline[Index] = hash;
            writer.WriteByte(Index);
            writer.WriteUShort(value);
        }
    }
}