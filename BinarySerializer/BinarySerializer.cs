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

        public static byte[] Serialize(object obj, Baseline baseline)
        {
            Type ownerType = obj.GetType();
            if (!Serializers.TryGetValue(ownerType, out IBinarySerializer item))
                item = Register(ownerType);

            using (BinaryDataWriter writer = new BinaryDataWriter())
            {
                item.Serialize(obj, writer, baseline);
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
            FieldInfo[] fields = ownerType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            List<IBinarySerializer> serializers = new List<IBinarySerializer>(fields.Length);
            for (byte index = 0; index < fields.Length; index++)
            {
                FieldInfo field = fields[index];
                BinaryItemAttribute attr =
                    (BinaryItemAttribute) field.GetCustomAttribute(typeof(BinaryItemAttribute), true);
                if (attr == null)
                    continue;

                Type fieldType = field.FieldType;

                if (fieldType.IsPrimitive)
                {
                    if (fieldType == typeof(bool))
                        serializers.Add(new BoolBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(byte))
                        serializers.Add(new ByteBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(sbyte))
                        serializers.Add(new SByteBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(short))
                        serializers.Add(new ShortBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(ushort))
                        serializers.Add(new UShortBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(int))
                        serializers.Add(new IntBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(uint))
                        serializers.Add(new UIntBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(long))
                        serializers.Add(new LongBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(ulong))
                        serializers.Add(new ULongBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(double))
                        serializers.Add(new DoubleBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(char))
                        serializers.Add(new CharBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(float))
                        serializers.Add(attr.IsShort
                            ? (IBinarySerializer) new ShortFloatBinarySerializer(index, ownerType, field)
                            : new FloatBinarySerializer(index, ownerType, field));
                    else throw new ArgumentException();
                }
                else if (fieldType.IsEnum)
                {
                    Type underlyingType = Enum.GetUnderlyingType(fieldType);
                    if (underlyingType == typeof(byte))
                        serializers.Add(new ByteEnumBinarySerializer(index, ownerType, field));
                    else if (underlyingType == typeof(int))
                        serializers.Add(new IntEnumBinarySerializer(index, ownerType, field));
                    else throw new ArgumentException("Not supported enum underlying type: " + underlyingType.Name);
                }
                else if (fieldType == typeof(string))
                {
                    serializers.Add(new StringBinarySerializer(index, ownerType, field));
                }
                else if (typeof(IProperty).IsAssignableFrom(fieldType))
                {
                    if (fieldType == typeof(Property<bool>))
                        serializers.Add(new BoolPropertyBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(Property<byte>))
                        serializers.Add(new BytePropertyBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(Property<sbyte>))
                        serializers.Add(new SBytePropertyBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(Property<short>))
                        serializers.Add(new ShortPropertyBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(Property<ushort>))
                        serializers.Add(new UShortPropertyBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(Property<int>))
                        serializers.Add(new IntPropertyBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(Property<uint>))
                        serializers.Add(new UIntPropertyBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(Property<long>))
                        serializers.Add(new LongPropertyBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(Property<ulong>))
                        serializers.Add(new ULongPropertyBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(Property<double>))
                        serializers.Add(new DoublePropertyBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(Property<char>))
                        serializers.Add(new CharPropertyBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(Property<string>))
                        serializers.Add(new StringPropertyBinarySerializer(index, ownerType, field));
                    else if (fieldType == typeof(Property<float>))
                        serializers.Add(attr.IsShort
                            ? (IBinarySerializer) new ShortFloatPropertyBinarySerializer(index, ownerType, field)
                            : new FloatPropertyBinarySerializer(index, ownerType, field));
                    else throw new ArgumentException();
                }
                else if (fieldType.IsClass)
                {
                    if (Serializers.TryGetValue(fieldType, out IBinarySerializer childSer)) 
                        continue;
                    
                    if (typeof(IBinaryObjectCollection).IsAssignableFrom(fieldType))
                    {
                        Type valueType = fieldType.GenericTypeArguments[0];
                        if (!Serializers.TryGetValue(valueType, out IBinarySerializer valueSer))
                            valueSer = Register(valueType);

                        if (!Creators.TryGetValue(valueType, out Creator itemCreator))
                        {
                            itemCreator = new Creator(valueType.GetConstructor(new Type[] { }));
                            Creators.Add(valueType, itemCreator);
                        }

                        if (typeof(IBinaryObjectCollection<byte>).IsAssignableFrom(fieldType))
                        {
                            childSer = new DictionaryByteKeyBinarySerializer(itemCreator, valueSer);
                            serializers.Add(new WrapperBinarySerializer<byte>(index, ownerType, field, childSer));
                        }
                        else if (typeof(IBinaryObjectCollection<short>).IsAssignableFrom(fieldType))
                        {
                            childSer = new DictionaryShortKeyBinarySerializer(itemCreator, valueSer);
                            serializers.Add(new WrapperBinarySerializer<short>(index, ownerType, field, childSer));
                        }
                        else if (typeof(IBinaryObjectCollection<ushort>).IsAssignableFrom(fieldType))
                        {
                            childSer = new DictionaryUShortKeyBinarySerializer(itemCreator, valueSer);
                            serializers.Add(new WrapperBinarySerializer<ushort>(index, ownerType, field, childSer));
                        }
                        else if (typeof(IBinaryObjectCollection<int>).IsAssignableFrom(fieldType))
                        {
                            childSer = new DictionaryIntKeyBinarySerializer(itemCreator, valueSer);
                            serializers.Add(new WrapperBinarySerializer<int>(index, ownerType, field, childSer));
                        }
                        else if (typeof(IBinaryObjectCollection<uint>).IsAssignableFrom(fieldType))
                        {
                            childSer = new DictionaryUIntKeyBinarySerializer(itemCreator, valueSer);
                            serializers.Add(new WrapperBinarySerializer<uint>(index, ownerType, field, childSer));
                        }
                        else if (typeof(IBinaryObjectCollection<long>).IsAssignableFrom(fieldType))
                        {
                            childSer = new DictionaryLongKeyBinarySerializer(itemCreator, valueSer);
                            serializers.Add(new WrapperBinarySerializer<long>(index, ownerType, field, childSer));
                        }
                        else if (typeof(IBinaryObjectCollection<ulong>).IsAssignableFrom(fieldType))
                        {
                            childSer = new DictionaryULongKeyBinarySerializer(itemCreator, valueSer);
                            serializers.Add(new WrapperBinarySerializer<ulong>(index, ownerType, field, childSer));
                        }
                        else throw new ArgumentException();

                    }
                    else if (attr.CustomSerializer != null)
                    {
                        if (!Creators.TryGetValue(attr.CustomSerializer, out Creator creator))
                        {
                            creator = new Creator(attr.CustomSerializer.GetConstructor(new[] {Serializers.GetType()}));
                            Creators.Add(attr.CustomSerializer, creator);
                        }

                        childSer = (ICustomBinarySerializer) creator.Create(Serializers);
                        Serializers.Add(fieldType, childSer);

                        serializers.Add(new WrapperBinarySerializer<byte>(index, ownerType, field, childSer));
                    }
                    else
                    {
                        childSer = Register(fieldType);
                        serializers.Add(new WrapperBinarySerializer<byte>(index, ownerType, field, childSer));
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