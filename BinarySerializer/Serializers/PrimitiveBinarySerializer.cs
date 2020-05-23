using System;
using System.Reflection;
using BinarySerializer.Data;
using BinarySerializer.Expressions;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer.Serializers
{
    public abstract class PrimitiveBinarySerializer<T> : IBinarySerializer
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
            Serialize(obj, writer, (IBaseline<byte, byte>) baseline);
        }
        
        protected abstract void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline);
        
        public abstract void Update(object obj, BinaryDataReader reader);
        public abstract void Serialize(object obj, BinaryDataWriter writer);
    }
    
    public class BoolBinarySerializer : PrimitiveBinarySerializer<bool>
    {
        public BoolBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            bool value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out bool baseValue) && value == default || baseValue == value)
                return;

            baseline[Index] = value;

            writer.WriteByte(Index);
            writer.WriteBool(value);
        }
    }
    
    public class ByteBinarySerializer : PrimitiveBinarySerializer<byte>
    {
        public ByteBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            byte value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out byte baseValue) && value == default || baseValue == value)
                return;

            baseline[Index] = value;
            
            writer.WriteByte(Index);
            writer.WriteByte(value);
        }
    }
    
    public class CharBinarySerializer : PrimitiveBinarySerializer<char>
    {
        public CharBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            char value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out byte baseValue) && value == default || baseValue == value)
                return;

            baseline[Index] = value;
            
            writer.WriteByte(Index);
            writer.WriteChar(value);
        }
    }
    
    public class DoubleBinarySerializer : PrimitiveBinarySerializer<double>
    {
        public DoubleBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            double value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out double baseValue) && Math.Abs(value) < 1e-6 ||
                Math.Abs(baseValue - value) < 1e-6)
                return;

            baseline[Index] = value;

            writer.WriteByte(Index);
            writer.WriteDouble(value);
        }
    }
    
    public class FloatBinarySerializer : PrimitiveBinarySerializer<float>
    {
        public FloatBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            float value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out float baseValue) && Math.Abs(value) < 1e-6 ||
                Math.Abs(baseValue - value) < 1e-6)
                return;

            baseline[Index] = value;

            writer.WriteByte(Index);
            writer.WriteFloat(value);
        }
    }
    
    public class IntBinarySerializer : PrimitiveBinarySerializer<int>
    {
        public IntBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            int value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseValue) && value == default || baseValue == value)
                return;

            baseline[Index] = value;
            
            writer.WriteByte(Index);
            writer.WriteInt(value);
        }
    }
    
    public class LongBinarySerializer : PrimitiveBinarySerializer<long>
    {
        public LongBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            long value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out long baseValue) && value == default || baseValue == value)
                return;
            
            baseline[Index] = value;
            
            writer.WriteByte(Index);
            writer.WriteLong(value);
        }
    }
    
    public class SByteBinarySerializer : PrimitiveBinarySerializer<sbyte>
    {
        public SByteBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            sbyte value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out sbyte baseValue) && value == default || baseValue == value)
                return;

            baseline[Index] = value;
            
            writer.WriteByte(Index);
            writer.WriteSByte(value);
        }
    }
    
    public class ShortBinarySerializer : PrimitiveBinarySerializer<short>
    {
        public ShortBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            short value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out short baseValue) && value == default || baseValue == value)
                return;

            baseline[Index] = value;
            
            writer.WriteByte(Index);
            writer.WriteShort(value);
        }
    }
    
    public class ShortFloatBinarySerializer : PrimitiveBinarySerializer<float>
    {
        public ShortFloatBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            float value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out float baseValue) && Math.Abs(value) < 1e-6 ||
                Math.Abs(baseValue - value) < 1e-6)
                return;

            baseline[Index] = value;

            writer.WriteByte(Index);
            writer.WriteShortFloat(value);
        }
    }
    
    public class StringBinarySerializer : PrimitiveBinarySerializer<string>
    {
        public StringBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            string value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out string baseValue) && string.IsNullOrEmpty(value) || baseValue == value)
                return;

            baseline[Index] = value;

            writer.WriteByte(Index);
            writer.WriteString(value);
        }
    }
    
    public class UIntBinarySerializer : PrimitiveBinarySerializer<uint>
    {
        public UIntBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            uint value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out uint baseValue) && value == default || baseValue == value)
                return;

            baseline[Index] = value;

            writer.WriteByte(Index);
            writer.WriteUInt(value);
        }
    }
    
    public class ULongBinarySerializer : PrimitiveBinarySerializer<ulong>
    {
        public ULongBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            ulong value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out ulong baseValue) && value == default || baseValue == value)
                return;

            baseline[Index] = value;

            writer.WriteByte(Index);
            writer.WriteULong(value);
        }
    }
    
    public class UShortBinarySerializer : PrimitiveBinarySerializer<ushort>
    {
        public UShortBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
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

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            ushort value = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out ushort baseValue) && value == default || baseValue == value)
                return;

            baseline[Index] = value;

            writer.WriteByte(Index);
            writer.WriteUShort(value);
        }
    }
}