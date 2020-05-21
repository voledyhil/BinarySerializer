using System;
using System.Reflection;
using BinarySerializer.Data;
using BinarySerializer.Expressions;

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
            writer.WriteByte(Index);
            writer.WriteBool(Getter.Get(obj));
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
            writer.WriteByte(Index);
            writer.WriteByte(Getter.Get(obj));
        }
    }
    
    public class CharBinarySerializer : PrimitiveBinarySerializer<char>
    {
        public CharBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            throw new NotImplementedException();
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
            writer.WriteByte(Index);
            writer.WriteDouble(Getter.Get(obj));
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
            writer.WriteByte(Index);
            writer.WriteFloat(Getter.Get(obj));
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
            writer.WriteByte(Index);
            writer.WriteInt(Getter.Get(obj));
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
            writer.WriteByte(Index);
            writer.WriteLong(Getter.Get(obj));
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
            writer.WriteByte(Index);
            writer.WriteSByte(Getter.Get(obj));
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
            writer.WriteByte(Index);
            writer.WriteShort(Getter.Get(obj));
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
            writer.WriteByte(Index);
            writer.WriteShortFloat(Getter.Get(obj));
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
            writer.WriteByte(Index);
            writer.WriteString(Getter.Get(obj));
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
            writer.WriteByte(Index);
            writer.WriteUInt(Getter.Get(obj));
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
            writer.WriteByte(Index);
            writer.WriteULong(Getter.Get(obj));
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
            writer.WriteByte(Index);
            writer.WriteUShort(Getter.Get(obj));
        }
    }
}