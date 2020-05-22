using System;
using System.Reflection;
using BinarySerializer.Data;
using BinarySerializer.Expressions;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer.Serializers
{
    public abstract class EnumBinarySerializer : IBinarySerializer
    {
        protected readonly byte Index;
        protected readonly FieldInfo Field;
        protected readonly Getter<object> Getter;
        protected readonly Setter<object> Setter;

        protected EnumBinarySerializer(byte index, Type ownerType, FieldInfo field)
        {
            Index = index;
            Field = field;
            Getter = new Getter<object>(ownerType, field);
            Setter = new Setter<object>(ownerType, field);
        }

        public abstract void Update(object obj, BinaryDataReader reader);
        public abstract void Serialize(object obj, BinaryDataWriter writer);
        public abstract void Serialize(object obj, BinaryDataWriter writer, Baseline baseline);
    }

    public class ByteEnumBinarySerializer : EnumBinarySerializer
    {
        public ByteEnumBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, Enum.ToObject(Field.FieldType, reader.ReadByte()));
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            byte value = (byte) Getter.Get(obj);
            if (value == default)
                return;

            writer.WriteByte(Index);
            writer.WriteByte(value);
        }

        public override void Serialize(object obj, BinaryDataWriter writer, Baseline baseline)
        {
            byte value = (byte) Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out byte baseValue) && value == default || baseValue == value)
                return;

            baseline[Index] = value;

            writer.WriteByte(Index);
            writer.WriteByte(value);
        }
    }

    public class IntEnumBinarySerializer : EnumBinarySerializer
    {
        public IntEnumBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Setter.Set(obj, Enum.ToObject(Field.FieldType, reader.ReadInt()));
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            int value = (int)Getter.Get(obj);
            if (value == default)
                return;
            
            writer.WriteByte(Index);
            writer.WriteInt(value);
        }
        
        public override void Serialize(object obj, BinaryDataWriter writer, Baseline baseline)
        {
            int value = (int) Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseValue) && value == default || baseValue == value)
                return;

            baseline[Index] = value;

            writer.WriteByte(Index);
            writer.WriteInt(value);
        }
    }
}