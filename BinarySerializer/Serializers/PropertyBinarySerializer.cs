using System;
using System.Reflection;
using BinarySerializer.Data;
using BinarySerializer.Expressions;
using BinarySerializer.Properties;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer.Serializers
{
    public abstract class PropertyBinarySerializer<T> : IBinarySerializer
    {
        protected readonly byte Index;
        protected readonly Getter<Property<T>> Getter;

        protected PropertyBinarySerializer(byte index, Type ownerType, FieldInfo field)
        {
            Index = index;
            Getter = new Getter<Property<T>>(ownerType, field);
        }
        
        void IBinarySerializer.Serialize(object obj, BinaryDataWriter writer, IBaseline baseline)
        {
            Serialize(obj, writer, (IBaseline<byte>) baseline);
        }

        protected abstract void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline);

        public abstract void Update(object obj, BinaryDataReader reader);
        public abstract void Serialize(object obj, BinaryDataWriter writer);
    }
    
    
    public class BoolPropertyBinarySerializer : PropertyBinarySerializer<bool>
    {
        public BoolPropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadBool());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<bool> property = Getter.Get(obj);
            if (property.Value == default)
                return;
            
            writer.WriteByte(Index);
            writer.WriteBool(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<bool> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteBool(property.Value);
        }
    }
    
    public class IntPropertyBinarySerializer : PropertyBinarySerializer<int>
    {
        public IntPropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadInt());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<int> property = Getter.Get(obj);
            if (property.Value == default)
                return;
            
            writer.WriteByte(Index);
            writer.WriteInt(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<int> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteInt(property.Value);
        }
    }
    
    public class BytePropertyBinarySerializer : PropertyBinarySerializer<byte>
    {
        public BytePropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadByte());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<byte> property = Getter.Get(obj);
            if (property.Value == default)
                return;
            
            writer.WriteByte(Index);
            writer.WriteByte(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<byte> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteByte(property.Value);
        }
    }
    
    public class CharPropertyBinarySerializer : PropertyBinarySerializer<char>
    {
        public CharPropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadChar());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<char> property = Getter.Get(obj);
            if (property.Value == default)
                return;
            
            writer.WriteByte(Index);
            writer.WriteChar(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<char> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteChar(property.Value);
        }
    }
    
    public class DoublePropertyBinarySerializer : PropertyBinarySerializer<double>
    {
        public DoublePropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadDouble());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<double> property = Getter.Get(obj);
            if (Math.Abs(property.Value) < 1e-6)
                return;

            writer.WriteByte(Index);
            writer.WriteDouble(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<double> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteDouble(property.Value);
        }
    }
    
    public class FloatPropertyBinarySerializer : PropertyBinarySerializer<float>
    {
        public FloatPropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadFloat());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<float> property = Getter.Get(obj);
            if (Math.Abs(property.Value) < 1e-6) 
                return;
            
            writer.WriteByte(Index);
            writer.WriteFloat(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<float> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteFloat(property.Value);
        }
    }
    
    public class LongPropertyBinarySerializer : PropertyBinarySerializer<long>
    {
        public LongPropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadLong());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<long> property = Getter.Get(obj);
            if (property.Value == default) 
                return;
            
            writer.WriteByte(Index);
            writer.WriteLong(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<long> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteLong(property.Value);
        }
    }
    
    public class SBytePropertyBinarySerializer : PropertyBinarySerializer<sbyte>
    {
        public SBytePropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadSByte());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<sbyte> property = Getter.Get(obj);
            if (property.Value == default) 
                return;
            
            writer.WriteByte(Index);
            writer.WriteSByte(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<sbyte> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteSByte(property.Value);
        }
    }
    
    public class ShortPropertyBinarySerializer : PropertyBinarySerializer<short>
    {
        public ShortPropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadShort());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<short> property = Getter.Get(obj);
            if (property.Value == default) 
                return;
            
            writer.WriteByte(Index);
            writer.WriteShort(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<short> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteShort(property.Value);
        }
    }
    
    public class ShortFloatPropertyBinarySerializer : PropertyBinarySerializer<float>
    {
        public ShortFloatPropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadShortFloat());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<float> property = Getter.Get(obj);
            if (Math.Abs(property.Value) < 1e-6) 
                return;
            
            writer.WriteByte(Index);
            writer.WriteShortFloat(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<float> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteShortFloat(property.Value);
        }
    }
    
    public class StringPropertyBinarySerializer : PropertyBinarySerializer<string>
    {
        public StringPropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadString());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<string> property = Getter.Get(obj);
            if (string.IsNullOrEmpty(property.Value)) 
                return;
            
            writer.WriteByte(Index);
            writer.WriteString(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<string> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteString(property.Value);
        }
    }
    
    public class UIntPropertyBinarySerializer : PropertyBinarySerializer<uint>
    {
        public UIntPropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadUInt());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<uint> property = Getter.Get(obj);
            if (property.Value == default) 
                return;
            
            writer.WriteByte(Index);
            writer.WriteUInt(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<uint> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteUInt(property.Value);
        }
    }
    
    public class ULongPropertyBinarySerializer : PropertyBinarySerializer<ulong>
    {
        public ULongPropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadULong());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<ulong> property = Getter.Get(obj);
            if (property.Value == default) 
                return;
            
            writer.WriteByte(Index);
            writer.WriteULong(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<ulong> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteULong(property.Value);
        }
    }
    
    public class UShortPropertyBinarySerializer : PropertyBinarySerializer<ushort>
    {
        public UShortPropertyBinarySerializer(byte index, Type ownerType, FieldInfo field) : base(index, ownerType, field)
        {
        }

        public override void Update(object obj, BinaryDataReader reader)
        {
            Getter.Get(obj).Update(reader.ReadUShort());
        }

        public override void Serialize(object obj, BinaryDataWriter writer)
        {
            Property<ushort> property = Getter.Get(obj);
            if (property.Value == default) 
                return;
            
            writer.WriteByte(Index);
            writer.WriteUShort(property.Value);
        }

        protected override void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Property<ushort> property = Getter.Get(obj);
            if (!baseline.TryGetValue(Index, out int baseVersion) && property.Version == 0 || baseVersion == property.Version)
                return;

            baseline[Index] = property.Version;
            
            writer.WriteByte(Index);
            writer.WriteUShort(property.Value);
        }
    }
}