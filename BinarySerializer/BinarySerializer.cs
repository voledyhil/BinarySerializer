using System;
using System.Collections.Generic;
using System.Reflection;
using BinarySerializer.Data;
using BinarySerializer.Expressions;
using BinarySerializer.Properties;
using BinarySerializer.Serializers;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer
{
    public static class BinarySerializer
    {
        private static readonly BoolWriter BoolWriter = new BoolWriter();
        private static readonly ByteWriter ByteWriter = new ByteWriter();
        private static readonly SByteWriter SbyteWriter = new SByteWriter();
        private static readonly ShortWriter ShortWriter = new ShortWriter();
        private static readonly UShortWriter UShortWriter = new UShortWriter();
        private static readonly IntWriter IntWriter = new IntWriter();
        private static readonly UIntWriter UIntWriter = new UIntWriter();
        private static readonly LongWriter LongWriter = new LongWriter();
        private static readonly ULongWriter UlongWriter = new ULongWriter();
        private static readonly DoubleWriter DoubleWriter = new DoubleWriter();
        private static readonly CharWriter CharWriter = new CharWriter();
        private static readonly FloatWriter FloatWriter = new FloatWriter();
        private static readonly ShortFloatWriter ShortFloatWriter = new ShortFloatWriter();
        private static readonly StringWriter StringWriter = new StringWriter();

        private static readonly IDictionary<Type, CompositeBinarySerializer> Serializers = new Dictionary<Type, CompositeBinarySerializer>();
        private static readonly IDictionary<Type, ObjectActivator> Creators = new Dictionary<Type, ObjectActivator>();

        public static byte[] Serialize(object obj)
        {
            using (BinaryDataWriter writer = new BinaryDataWriter())
            {
                GetSerializer(obj.GetType()).Serialize(obj, writer);
                return writer.GetData();
            }
        }

        public static byte[] Serialize(object obj, Baseline<byte> baseline)
        {
            CompositeBinarySerializer serializer = GetSerializer(obj.GetType());

            if (!baseline.HasValues)
                baseline.CreateValues(serializer.Count);

            using (BinaryDataWriter writer = new BinaryDataWriter())
            {
                serializer.Serialize(obj, writer, baseline);
                return writer.GetData();
            }
        }

        public static void Deserialize(object obj, byte[] data)
        {
            using (BinaryDataReader reader = new BinaryDataReader(data))
            {
                GetSerializer(obj.GetType()).Update(obj, reader);
            }
        }

        public static CompositeBinarySerializer GetSerializer(Type type)
        {
            return Serializers.TryGetValue(type, out CompositeBinarySerializer item) ? item : RegisterType(type);
        }

        public static CompositeBinarySerializer RegisterType(Type ownerType)
        {
            if (Serializers.TryGetValue(ownerType, out CompositeBinarySerializer item))
                return item;
            
            FieldInfo[] fields = ownerType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            Dictionary<byte, IBinarySerializer> serializers = new Dictionary<byte, IBinarySerializer>(fields.Length);
            for (byte i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                BinaryItemAttribute attr =
                    (BinaryItemAttribute) field.GetCustomAttribute(typeof(BinaryItemAttribute), true);
                if (attr == null)
                    continue;

                Type fieldType = field.FieldType;

                if (fieldType.IsPrimitive)
                {
                    if (fieldType == typeof(bool))
                        serializers.Add(attr.Id, new BoolBinarySerializer(attr.Id, ownerType, field, BoolWriter));
                    else if (fieldType == typeof(byte))
                        serializers.Add(attr.Id, new ByteBinarySerializer(attr.Id, ownerType, field, ByteWriter));
                    else if (fieldType == typeof(sbyte))
                        serializers.Add(attr.Id, new SByteBinarySerializer(attr.Id, ownerType, field, SbyteWriter));
                    else if (fieldType == typeof(short))
                        serializers.Add(attr.Id, new ShortBinarySerializer(attr.Id, ownerType, field, ShortWriter));
                    else if (fieldType == typeof(ushort))
                        serializers.Add(attr.Id, new UShortBinarySerializer(attr.Id, ownerType, field, UShortWriter));
                    else if (fieldType == typeof(int))
                        serializers.Add(attr.Id, new IntBinarySerializer(attr.Id, ownerType, field, IntWriter));
                    else if (fieldType == typeof(uint))
                        serializers.Add(attr.Id, new UIntBinarySerializer(attr.Id, ownerType, field, UIntWriter));
                    else if (fieldType == typeof(long))
                        serializers.Add(attr.Id, new LongBinarySerializer(attr.Id, ownerType, field, LongWriter));
                    else if (fieldType == typeof(ulong))
                        serializers.Add(attr.Id, new ULongBinarySerializer(attr.Id, ownerType, field, UlongWriter));
                    else if (fieldType == typeof(double))
                        serializers.Add(attr.Id, new DoubleBinarySerializer(attr.Id, ownerType, field, DoubleWriter));
                    else if (fieldType == typeof(char))
                        serializers.Add(attr.Id, new CharBinarySerializer(attr.Id, ownerType, field, CharWriter));
                    else if (fieldType == typeof(float))
                        serializers.Add(attr.Id, attr.IsShort
                            ? (IBinarySerializer) new ShortFloatBinarySerializer(attr.Id, ownerType, field,
                                ShortFloatWriter)
                            : new FloatBinarySerializer(attr.Id, ownerType, field, FloatWriter));
                    else throw new ArgumentException();
                }
                else if (fieldType.IsEnum)
                {
                    Type underlyingType = Enum.GetUnderlyingType(fieldType);
                    if (underlyingType == typeof(byte))
                        serializers.Add(attr.Id, new ByteEnumBinarySerializer(attr.Id, ownerType, field));
                    else if (underlyingType == typeof(int))
                        serializers.Add(attr.Id, new IntEnumBinarySerializer(attr.Id, ownerType, field));
                    else throw new ArgumentException("Not supported enum underlying type: " + underlyingType.Name);
                }
                else if (fieldType == typeof(string))
                {
                    serializers.Add(attr.Id, new StringBinarySerializer(attr.Id, ownerType, field, StringWriter));
                }
                else if (typeof(IProperty).IsAssignableFrom(fieldType))
                {
                    if (fieldType == typeof(Property<bool>))
                        serializers.Add(attr.Id, new BoolPropertyBinarySerializer(attr.Id, ownerType, field, BoolWriter));
                    else if (fieldType == typeof(Property<byte>))
                        serializers.Add(attr.Id, new BytePropertyBinarySerializer(attr.Id, ownerType, field, ByteWriter));
                    else if (fieldType == typeof(Property<sbyte>))
                        serializers.Add(attr.Id, new SBytePropertyBinarySerializer(attr.Id, ownerType, field, SbyteWriter));
                    else if (fieldType == typeof(Property<short>))
                        serializers.Add(attr.Id, new ShortPropertyBinarySerializer(attr.Id, ownerType, field, ShortWriter));
                    else if (fieldType == typeof(Property<ushort>))
                        serializers.Add(attr.Id, new UShortPropertyBinarySerializer(attr.Id, ownerType, field, UShortWriter));
                    else if (fieldType == typeof(Property<int>))
                        serializers.Add(attr.Id, new IntPropertyBinarySerializer(attr.Id, ownerType, field, IntWriter));
                    else if (fieldType == typeof(Property<uint>))
                        serializers.Add(attr.Id, new UIntPropertyBinarySerializer(attr.Id, ownerType, field, UIntWriter));
                    else if (fieldType == typeof(Property<long>))
                        serializers.Add(attr.Id, new LongPropertyBinarySerializer(attr.Id, ownerType, field, LongWriter));
                    else if (fieldType == typeof(Property<ulong>))
                        serializers.Add(attr.Id, new ULongPropertyBinarySerializer(attr.Id, ownerType, field, UlongWriter));
                    else if (fieldType == typeof(Property<double>))
                        serializers.Add(attr.Id, new DoublePropertyBinarySerializer(attr.Id, ownerType, field, DoubleWriter));
                    else if (fieldType == typeof(Property<char>))
                        serializers.Add(attr.Id, new CharPropertyBinarySerializer(attr.Id, ownerType, field, CharWriter));
                    else if (fieldType == typeof(Property<string>))
                        serializers.Add(attr.Id, new StringPropertyBinarySerializer(attr.Id, ownerType, field, StringWriter));
                    else if (fieldType == typeof(Property<float>))
                        serializers.Add(attr.Id, attr.IsShort
                            ? (IBinarySerializer) new ShortFloatPropertyBinarySerializer(attr.Id, ownerType, field,
                                ShortFloatWriter)
                            : new FloatPropertyBinarySerializer(attr.Id, ownerType, field, FloatWriter));
                    else throw new ArgumentException();
                }
                else if (fieldType.IsClass)
                {
                    if (Serializers.TryGetValue(fieldType, out CompositeBinarySerializer ser))
                    {
                        serializers.Add(attr.Id, new ByteWrapperBinarySerializer<byte>(attr.Id, ownerType, field, ser, ser.Count));
                    }
                    else if (typeof(IBinaryObjectCollection).IsAssignableFrom(fieldType))
                    {
                        Type valueType = fieldType.GenericTypeArguments[0];
                        CompositeBinarySerializer valueSer = GetSerializer(valueType);

                        if (!Creators.TryGetValue(valueType, out ObjectActivator itemCreator))
                        {
                            itemCreator = Expressions.Expressions.InstantiateCreator(valueType.GetConstructor(new Type[] { }));
                            Creators.Add(valueType, itemCreator);
                        }

                        if (typeof(IBinaryObjectCollection<byte>).IsAssignableFrom(fieldType))
                            serializers.Add(attr.Id, new ByteWrapperBinarySerializer<byte>(attr.Id, ownerType, field,
                                new DictionaryByteKeyBinarySerializer(itemCreator, valueSer, ByteWriter)));
                        else if (typeof(IBinaryObjectCollection<short>).IsAssignableFrom(fieldType))
                            serializers.Add(attr.Id, new ByteWrapperBinarySerializer<short>(attr.Id, ownerType, field,
                                new DictionaryShortKeyBinarySerializer(itemCreator, valueSer, ShortWriter)));
                        else if (typeof(IBinaryObjectCollection<ushort>).IsAssignableFrom(fieldType))
                            serializers.Add(attr.Id, new ByteWrapperBinarySerializer<ushort>(attr.Id, ownerType, field,
                                new DictionaryUShortKeyBinarySerializer(itemCreator, valueSer, UShortWriter)));
                        else if (typeof(IBinaryObjectCollection<int>).IsAssignableFrom(fieldType))
                            serializers.Add(attr.Id, new ByteWrapperBinarySerializer<int>(attr.Id, ownerType, field,
                                new DictionaryIntKeyBinarySerializer(itemCreator, valueSer, IntWriter)));
                        else if (typeof(IBinaryObjectCollection<uint>).IsAssignableFrom(fieldType))
                            serializers.Add(attr.Id, new ByteWrapperBinarySerializer<uint>(attr.Id, ownerType, field,
                                new DictionaryUIntKeyBinarySerializer(itemCreator, valueSer, UIntWriter)));
                        else if (typeof(IBinaryObjectCollection<long>).IsAssignableFrom(fieldType))
                            serializers.Add(attr.Id, new ByteWrapperBinarySerializer<long>(attr.Id, ownerType, field,
                                new DictionaryLongKeyBinarySerializer(itemCreator, valueSer, LongWriter)));
                        else if (typeof(IBinaryObjectCollection<ulong>).IsAssignableFrom(fieldType))
                            serializers.Add(attr.Id, new ByteWrapperBinarySerializer<ulong>(attr.Id, ownerType, field,
                                new DictionaryULongKeyBinarySerializer(itemCreator, valueSer, UlongWriter)));
                        else throw new ArgumentException();
                    }
                    else
                    {
                        ser = RegisterType(fieldType);
                        serializers.Add(attr.Id, new ByteWrapperBinarySerializer<byte>(attr.Id, ownerType, field, ser, ser.Count));
                    }
                }
                else throw new ArgumentException();
            }

            CompositeBinarySerializer serializer = new CompositeBinarySerializer(serializers);
            Serializers.Add(ownerType, new CompositeBinarySerializer(serializers));
            return serializer;
        }
    }
}