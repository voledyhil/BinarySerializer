using System;
using BinarySerializer.Properties;

namespace BinarySerializer.Tests
{
    public enum ByteEnum : byte
    {
        First = 0,
        Second = 1
    }
    
    public enum IntEnum : int
    {
        First = 0,
        Second = 1
    }

    public class ParentMock : IEquatable<ParentMock>
    {
        [BinaryItem] public bool Bool;
        [BinaryItem] public byte Byte;
        [BinaryItem] public sbyte Sbyte;
        [BinaryItem] public short Short;
        [BinaryItem] public ushort UShort;
        [BinaryItem] public int Int;
        [BinaryItem] public uint UInt;
        [BinaryItem] public long Long;
        [BinaryItem] public ulong ULong;
        [BinaryItem] public double Double;
        [BinaryItem] public char Char;
        [BinaryItem] public float Float;
        [BinaryItem(true)] public float ShortFloat;
        [BinaryItem] public string String;
        [BinaryItem] public ByteEnum ByteEnum;
        [BinaryItem] public IntEnum IntEnum;
        [BinaryItem] public Property<bool> BoolProperty = new Property<bool>();
        [BinaryItem] public Property<byte> ByteProperty = new Property<byte>();
        [BinaryItem] public Property<sbyte> SbyteProperty = new Property<sbyte>();
        [BinaryItem] public Property<short> ShortProperty = new Property<short>();
        [BinaryItem] public Property<ushort> UShortProperty = new Property<ushort>();
        [BinaryItem] public Property<int> IntProperty = new Property<int>();
        [BinaryItem] public Property<uint> UIntProperty = new Property<uint>();
        [BinaryItem] public Property<long> LongProperty = new Property<long>();
        [BinaryItem] public Property<ulong> ULongProperty = new Property<ulong>();
        [BinaryItem] public Property<double> DoubleProperty = new Property<double>();
        [BinaryItem] public Property<char> CharProperty = new Property<char>();
        [BinaryItem] public Property<float> FloatProperty = new Property<float>();
        [BinaryItem(true)] public Property<float> ShortFloatProperty = new Property<float>();

        public bool Equals(ParentMock other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Bool == other.Bool && Byte == other.Byte && Sbyte == other.Sbyte && Short == other.Short &&
                   UShort == other.UShort && Int == other.Int && UInt == other.UInt && Long == other.Long &&
                   ULong == other.ULong && Double.Equals(other.Double) && Char == other.Char &&
                   Float.Equals(other.Float) && ShortFloat.Equals(other.ShortFloat) &&
                   string.Equals(String, other.String) && ByteEnum == other.ByteEnum && IntEnum == other.IntEnum &&
                   Equals(BoolProperty, other.BoolProperty) && Equals(ByteProperty, other.ByteProperty) &&
                   Equals(SbyteProperty, other.SbyteProperty) && Equals(ShortProperty, other.ShortProperty) &&
                   Equals(UShortProperty, other.UShortProperty) && Equals(IntProperty, other.IntProperty) &&
                   Equals(UIntProperty, other.UIntProperty) && Equals(LongProperty, other.LongProperty) &&
                   Equals(ULongProperty, other.ULongProperty) && Equals(DoubleProperty, other.DoubleProperty) &&
                   Equals(CharProperty, other.CharProperty) && Equals(FloatProperty, other.FloatProperty) &&
                   Equals(ShortFloatProperty, other.ShortFloatProperty);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ParentMock) obj);
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
                hashCode = (hashCode * 397) ^ (int) ByteEnum;
                hashCode = (hashCode * 397) ^ (int) IntEnum;
                hashCode = (hashCode * 397) ^ (BoolProperty != null ? BoolProperty.GetHashCode() : 0);
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
                return hashCode;
            }
        }
    }
}