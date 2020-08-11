using System;
using BinarySerializer.Properties;
using BinarySerializer.Serializers;

namespace BinarySerializer.Tests
{   
    public class ParentMock : IEquatable<ParentMock>
    {
        [BinaryItem(0)] public ChildMock ChildMock = new ChildMock();

        public bool Equals(ParentMock other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || Equals(ChildMock, other.ChildMock);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ParentMock) obj);
        }

        public override int GetHashCode()
        {
            return ChildMock != null ? ChildMock.GetHashCode() : 0;
        }
    }
    
    public class ChildMock : IEquatable<ChildMock>
    {
        [BinaryItem(3)] public bool Bool;
        [BinaryItem(1)] public int Int;

        public bool Equals(ChildMock other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Bool == other.Bool && Int == other.Int;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ChildMock) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Bool.GetHashCode() * 397) ^ Int;
            }
        }
    }

    public class PrimitivesMock : IEquatable<PrimitivesMock>
    {
        [BinaryItem(0)] public bool Bool;
        [BinaryItem(10)] public byte Byte;
        [BinaryItem(20)] public sbyte Sbyte;
        [BinaryItem(30)] public short Short;
        [BinaryItem(40)] public ushort UShort;
        [BinaryItem(50)] public int Int;
        [BinaryItem(60)] public uint UInt;
        [BinaryItem(70)] public long Long;
        [BinaryItem(80)] public ulong ULong;
        [BinaryItem(90)] public double Double;
        [BinaryItem(100)] public char Char;
        [BinaryItem(110)] public float Float;
        [BinaryItem(120, true)] public float ShortFloat;
        [BinaryItem(130)] public string String;

        public bool Equals(PrimitivesMock other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Bool == other.Bool && Byte == other.Byte && Sbyte == other.Sbyte && Short == other.Short &&
                   UShort == other.UShort && Int == other.Int && UInt == other.UInt && Long == other.Long &&
                   ULong == other.ULong && Double.Equals(other.Double) && Char == other.Char &&
                   Float.Equals(other.Float) && ShortFloat.Equals(other.ShortFloat) &&
                   string.Equals(String, other.String);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((PrimitivesMock) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Bool.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte.GetHashCode();
                hashCode = (hashCode * 397) ^ Sbyte.GetHashCode();
                hashCode = (hashCode * 397) ^ Short.GetHashCode();
                hashCode = (hashCode * 397) ^ UShort.GetHashCode();
                hashCode = (hashCode * 397) ^ Int;
                hashCode = (hashCode * 397) ^ (int) UInt;
                hashCode = (hashCode * 397) ^ Long.GetHashCode();
                hashCode = (hashCode * 397) ^ ULong.GetHashCode();
                hashCode = (hashCode * 397) ^ Double.GetHashCode();
                hashCode = (hashCode * 397) ^ Char.GetHashCode();
                hashCode = (hashCode * 397) ^ Float.GetHashCode();
                hashCode = (hashCode * 397) ^ ShortFloat.GetHashCode();
                hashCode = (hashCode * 397) ^ (String != null ? String.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    
    public enum ByteEnum : byte
    {
        First = 0,
        Second = 1
    }

    public enum IntEnum
    {
        First = 0,
        Second = 1
    }
    
    public class EnumsMock : IEquatable<EnumsMock>
    {
        [BinaryItem(0)] public ByteEnum ByteEnum;
        [BinaryItem(1)] public IntEnum IntEnum;

        public bool Equals(EnumsMock other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ByteEnum == other.ByteEnum && IntEnum == other.IntEnum;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((EnumsMock) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) ByteEnum * 397) ^ (int) IntEnum;
            }
        }
    }

    public class PropertiesMock : IEquatable<PropertiesMock>
    {
        [BinaryItem(0)] public Property<bool> BoolProperty = new Property<bool>();
        [BinaryItem(10)] public Property<byte> ByteProperty = new Property<byte>();
        [BinaryItem(20)] public Property<sbyte> SbyteProperty = new Property<sbyte>();
        [BinaryItem(30)] public Property<short> ShortProperty = new Property<short>();
        [BinaryItem(40)] public Property<ushort> UShortProperty = new Property<ushort>();
        [BinaryItem(50)] public Property<int> IntProperty = new Property<int>();
        [BinaryItem(60)] public Property<uint> UIntProperty = new Property<uint>();
        [BinaryItem(70)] public Property<long> LongProperty = new Property<long>();
        [BinaryItem(80)] public Property<ulong> ULongProperty = new Property<ulong>();
        [BinaryItem(90)] public Property<double> DoubleProperty = new Property<double>();
        [BinaryItem(100)] public Property<char> CharProperty = new Property<char>();
        [BinaryItem(110)] public Property<float> FloatProperty = new Property<float>();
        [BinaryItem(120, true)] public Property<float> ShortFloatProperty = new Property<float>();
        [BinaryItem(130)] public Property<string> StringProperty = new Property<string>();

        public bool Equals(PropertiesMock other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(BoolProperty, other.BoolProperty) && Equals(ByteProperty, other.ByteProperty) && Equals(SbyteProperty, other.SbyteProperty) && Equals(ShortProperty, other.ShortProperty) && Equals(UShortProperty, other.UShortProperty) && Equals(IntProperty, other.IntProperty) && Equals(UIntProperty, other.UIntProperty) && Equals(LongProperty, other.LongProperty) && Equals(ULongProperty, other.ULongProperty) && Equals(DoubleProperty, other.DoubleProperty) && Equals(CharProperty, other.CharProperty) && Equals(FloatProperty, other.FloatProperty) && Equals(ShortFloatProperty, other.ShortFloatProperty) && Equals(StringProperty, other.StringProperty);
            
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PropertiesMock) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (BoolProperty != null ? BoolProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ByteProperty != null ? ByteProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SbyteProperty != null ? SbyteProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ShortProperty != null ? ShortProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UShortProperty != null ? UShortProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (IntProperty != null ? IntProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UIntProperty != null ? UIntProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LongProperty != null ? LongProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ULongProperty != null ? ULongProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DoubleProperty != null ? DoubleProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CharProperty != null ? CharProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FloatProperty != null ? FloatProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ShortFloatProperty != null ? ShortFloatProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StringProperty != null ? StringProperty.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    public class CollectionsMock
    {
        [BinaryItem(0)]
        public ByteBinaryObjectCollection<ChildMock> ByteObjects = new ByteBinaryObjectCollection<ChildMock>();
        [BinaryItem(10)]
        public UShortBinaryObjectCollection<ChildMock> UShortObjects = new UShortBinaryObjectCollection<ChildMock>();
        [BinaryItem(20)]
        public ShortBinaryObjectCollection<ChildMock> ShortObjects = new ShortBinaryObjectCollection<ChildMock>();
        [BinaryItem(30)]
        public IntBinaryObjectCollection<ChildMock> IntObjects = new IntBinaryObjectCollection<ChildMock>();
        [BinaryItem(40)]
        public UIntBinaryObjectCollection<ChildMock> UIntObjects = new UIntBinaryObjectCollection<ChildMock>();
        [BinaryItem(50)]
        public LongBinaryObjectCollection<ChildMock> LongObjects = new LongBinaryObjectCollection<ChildMock>();
        [BinaryItem(60)]
        public ULongBinaryObjectCollection<ChildMock> ULongObjects = new ULongBinaryObjectCollection<ChildMock>();
    }
}