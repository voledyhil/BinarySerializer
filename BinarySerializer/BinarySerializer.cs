using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BinarySerializer.Data;
using BinarySerializer.Expressions;
using BinarySerializer.Properties;
using BinarySerializer.Serializers;

namespace BinarySerializer
{
    public static class BinarySerializer
    {
        private static readonly IDictionary<Type, IBinarySerializer> Serializers = new Dictionary<Type, IBinarySerializer>();
        private static readonly IDictionary<Type, Creator> Creators = new Dictionary<Type, Creator>();

        public static byte[] Serialize(object obj)
        {
            Type ownerType = obj.GetType();
            if (!Serializers.TryGetValue(ownerType, out IBinarySerializer item))
                item = Register(ownerType);

            using (BinaryDataWriter writer = new BinaryDataWriter())
            {
                item.Serialize(obj, writer);
                return writer.GetData();
            }
        }

        public static void Deserialize(object obj, byte[] data)
        {
            Type ownerType = obj.GetType();
            if (!Serializers.TryGetValue(ownerType, out IBinarySerializer item))
                item = Register(ownerType);

            using (BinaryDataReader reader = new BinaryDataReader(data))
            {
                item.Update(obj, reader);
            }
        }

        private static IBinarySerializer Register(Type ownerType)
        {
            FieldInfo[] fields =
                ownerType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            List<IBinarySerializer> serializers = new List<IBinarySerializer>(fields.Length);
            for (byte index = 0; index < fields.Length; index++)
            {
                FieldInfo field = fields[index];
                BinaryItemAttribute attr =
                    (BinaryItemAttribute) field.GetCustomAttribute(typeof(BinaryItemAttribute), true);
                if (attr == null)
                    continue;

                Type fieldType = field.FieldType;

                IBinarySerializer ser;
                if (fieldType.IsPrimitive)
                {
                    if (fieldType == typeof(bool))
                        ser = new BoolBinarySerializer(index, ownerType, field);
                    else if (fieldType == typeof(byte))
                        ser = new ByteBinarySerializer(index, ownerType, field);
                    else if (fieldType == typeof(sbyte))
                        ser = new SByteBinarySerializer(index, ownerType, field);
                    else if (fieldType == typeof(short))
                        ser = new ShortBinarySerializer(index, ownerType, field);
                    else if (fieldType == typeof(ushort))
                        ser = new UShortBinarySerializer(index, ownerType, field);
                    else if (fieldType == typeof(int))
                        ser = new IntBinarySerializer(index, ownerType, field);
                    else if (fieldType == typeof(uint))
                        ser = new UIntBinarySerializer(index, ownerType, field);
                    else if (fieldType == typeof(long))
                        ser = new LongBinarySerializer(index, ownerType, field);
                    else if (fieldType == typeof(ulong))
                        ser = new ULongBinarySerializer(index, ownerType, field);
                    else if (fieldType == typeof(double))
                        ser = new DoubleBinarySerializer(index, ownerType, field);
                    else if (fieldType == typeof(char))
                        ser = new CharBinarySerializer(index, ownerType, field);
                    else if (fieldType == typeof(float))
                        ser = attr.IsShort
                            ? (IBinarySerializer) new ShortFloatBinarySerializer(index, ownerType, field)
                            : new FloatBinarySerializer(index, ownerType, field);
                    else throw new ArgumentException();
                }
                else if (fieldType.IsClass)
                {
                    if (typeof(IProperty).IsAssignableFrom(fieldType))
                    {
                        if (fieldType == typeof(Property<bool>))
                            ser = new BoolPropertyBinarySerializer(index, ownerType, field);
                        else if (fieldType == typeof(Property<byte>))
                            ser = new BytePropertyBinarySerializer(index, ownerType, field);
                        else if (fieldType == typeof(Property<sbyte>))
                            ser = new SBytePropertyBinarySerializer(index, ownerType, field);
                        else if (fieldType == typeof(Property<short>))
                            ser = new ShortPropertyBinarySerializer(index, ownerType, field);
                        else if (fieldType == typeof(Property<ushort>))
                            ser = new UShortPropertyBinarySerializer(index, ownerType, field);
                        else if (fieldType == typeof(Property<int>))
                            ser = new IntPropertyBinarySerializer(index, ownerType, field);
                        else if (fieldType == typeof(Property<uint>))
                            ser = new UIntPropertyBinarySerializer(index, ownerType, field);
                        else if (fieldType == typeof(Property<long>))
                            ser = new LongPropertyBinarySerializer(index, ownerType, field);
                        else if (fieldType == typeof(Property<ulong>))
                            ser = new ULongPropertyBinarySerializer(index, ownerType, field);
                        else if (fieldType == typeof(Property<double>))
                            ser = new DoublePropertyBinarySerializer(index, ownerType, field);
                        else if (fieldType == typeof(Property<char>))
                            ser = new CharPropertyBinarySerializer(index, ownerType, field);
                        else if (fieldType == typeof(Property<string>))
                            ser = new StringPropertyBinarySerializer(index, ownerType, field);
                        else if (fieldType == typeof(Property<float>))
                            ser = attr.IsShort
                                ? (IBinarySerializer) new ShortFloatPropertyBinarySerializer(index, ownerType, field)
                                : new FloatPropertyBinarySerializer(index, ownerType, field);
                        else throw new ArgumentException();
                    }
                    else
                    {
                        if (!Serializers.TryGetValue(fieldType, out IBinarySerializer childSer))
                        {
                            if (typeof(IDictionary).IsAssignableFrom(fieldType))
                            {
                                Type keyType = fieldType.GenericTypeArguments[0];
                                Type valueType = fieldType.GenericTypeArguments[1];

                                if (!Serializers.TryGetValue(valueType, out IBinarySerializer valueSer))
                                    valueSer = Register(valueType);

                                if (!Creators.TryGetValue(valueType, out Creator itemCreator))
                                {
                                    itemCreator = new Creator(valueType.GetConstructor(new Type[] { }));
                                    Creators.Add(valueType, itemCreator);
                                }

                                if (keyType == typeof(ushort))
                                    childSer = new DictionaryUShortKeyBinarySerializer(itemCreator, valueSer);
                                else if (fieldType == typeof(byte))
                                    childSer = new DictionaryByteKeyBinarySerializer(itemCreator, valueSer);
                                else if (fieldType == typeof(sbyte))
                                    childSer = new DictionarySByteKeyBinarySerializer(itemCreator, valueSer);
                                else if (fieldType == typeof(short))
                                    childSer = new DictionaryShortKeyBinarySerializer(itemCreator, valueSer);
                                else if (fieldType == typeof(ushort))
                                    childSer = new DictionaryUShortKeyBinarySerializer(itemCreator, valueSer);
                                else if (fieldType == typeof(int))
                                    childSer = new DictionaryIntKeyBinarySerializer(itemCreator, valueSer);
                                else if (fieldType == typeof(uint))
                                    childSer = new DictionaryUIntKeyBinarySerializer(itemCreator, valueSer);
                                else if (fieldType == typeof(long))
                                    childSer = new DictionaryLongKeyBinarySerializer(itemCreator, valueSer);
                                else if (fieldType == typeof(ulong))
                                    childSer = new DictionaryULongKeyBinarySerializer(itemCreator, valueSer);
                                else throw new ArgumentException();
                            }
                            else if (attr.CustomSerializer != null)
                            {
                                if (!Creators.TryGetValue(attr.CustomSerializer, out Creator creator))
                                {
                                    creator = new Creator(
                                        attr.CustomSerializer.GetConstructor(new[] {Serializers.GetType()}));
                                    Creators.Add(attr.CustomSerializer, creator);
                                }

                                childSer = (ICustomBinarySerializer) creator.Create(Serializers);
                                Serializers.Add(fieldType, childSer);
                            }
                            else
                            {
                                childSer = Register(fieldType);
                            }
                        }

                        ser = new WrapperBinarySerializer(index, ownerType, field, childSer);
                    }
                }
                else if (fieldType.IsEnum)
                {
                    Type underlyingType = Enum.GetUnderlyingType(fieldType);
                    if (underlyingType == typeof(byte))
                        ser = new ByteEnumBinarySerializer(index, ownerType, field);
                    else if (underlyingType == typeof(int))
                        ser = new IntEnumBinarySerializer(index, ownerType, field);
                    else throw new ArgumentException("Not supported enum underlying type: " + underlyingType.Name);
                }
                else if (fieldType == typeof(string))
                    ser = new StringBinarySerializer(index, ownerType, field);
                else throw new ArgumentException();

                serializers.Add(ser);
            }

            CompositeBinarySerializer serializer = new CompositeBinarySerializer(serializers);

            Serializers.Add(ownerType, serializer);

            return serializer;
        }
    }
}