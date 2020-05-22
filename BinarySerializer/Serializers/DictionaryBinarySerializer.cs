using System;
using System.Collections;
using System.Collections.Generic;
using BinarySerializer.Data;
using BinarySerializer.Expressions;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer.Serializers
{
    public abstract class DictionaryBinarySerializer : IBinarySerializer
    {
        private readonly int _keySize;
        private readonly object _reservedKey;
        private readonly Creator _itemCreator;
        private readonly IBinarySerializer _itemSerializer;

        protected DictionaryBinarySerializer(int keySize, object reservedKey, Creator itemCreator,
            IBinarySerializer itemSerializer)
        {
            _keySize = keySize;
            _reservedKey = reservedKey;
            _itemCreator = itemCreator;
            _itemSerializer = itemSerializer;
        }

        protected abstract object ReadKey(BinaryDataReader reader);
        protected abstract void WriteKey(object key, BinaryDataWriter writer);

        public void Update(object obj, BinaryDataReader reader)
        {
            IDictionary dict = (IDictionary) obj;

            while (reader.Position < reader.Length)
            {
                object key = ReadKey(reader);
                if (_reservedKey.Equals(key))
                    break;

                using (BinaryDataReader itemReader = reader.ReadNode())
                {
                    object value;
                    if (!dict.Contains(key))
                    {
                        value = _itemCreator.Create();
                        dict.Add(key, value);
                    }
                    else
                    {
                        value = dict[key];
                    }

                    _itemSerializer.Update(value, itemReader);
                }
            }

            while (reader.Position < reader.Length)
            {
                dict.Remove(ReadKey(reader));
            }
        }

        public void Serialize(object obj, BinaryDataWriter writer)
        {
            IDictionary dict = (IDictionary) obj;

            foreach (object key in dict.Keys)
            {
                if (_reservedKey.Equals(key))
                    throw new ArgumentException();
                
                BinaryDataWriter itemWriter = writer.TryWriteNode(_keySize);

                _itemSerializer.Serialize(dict[key], itemWriter);

                if (itemWriter.Length <= 0) 
                    continue;
                
                WriteKey(key, writer);
                itemWriter.PushNode();
            }
        }

        public void Serialize(object obj, BinaryDataWriter writer, Baseline baseline)
        {
            IDictionary dict = (IDictionary) obj;

            List<object> baseKeys = new List<object>(baseline.BaselineKeys);
            foreach (object key in dict.Keys)
            {
                object value = dict[key];
                BinaryDataWriter itemWriter = writer.TryWriteNode(_keySize);
                
                _itemSerializer.Serialize(value, itemWriter, baseline.GetOrCreateBaseline(key));

                if (itemWriter.Length > 0)
                {
                    WriteKey(key, writer);
                    itemWriter.PushNode();
                }
                
                baseKeys.Remove(key);
            }

            if (baseKeys.Count <= 0)
                return;

            WriteKey(_reservedKey, writer);
           
            foreach (object key in baseKeys)
            {
                WriteKey(key, writer);
                baseline.DestroyBaseline(key);
            }
        }
    }
    
    public class DictionaryByteKeyBinarySerializer : DictionaryBinarySerializer
    {
        public DictionaryByteKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(byte), byte.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override object ReadKey(BinaryDataReader reader)
        {
            return reader.ReadByte();
        }

        protected override void WriteKey(object key, BinaryDataWriter writer)
        {
            writer.WriteByte((byte) key);
        }
    }
    
    public class DictionarySByteKeyBinarySerializer : DictionaryBinarySerializer
    {
        public DictionarySByteKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(sbyte), sbyte.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override object ReadKey(BinaryDataReader reader)
        {
            return reader.ReadSByte();
        }

        protected override void WriteKey(object key, BinaryDataWriter writer)
        {
            writer.WriteSByte((sbyte) key);
        }
    }
    
    public class DictionaryShortKeyBinarySerializer : DictionaryBinarySerializer
    {
        public DictionaryShortKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(short), short.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override object ReadKey(BinaryDataReader reader)
        {
            return reader.ReadShort();
        }

        protected override void WriteKey(object key, BinaryDataWriter writer)
        {
            writer.WriteShort((short) key);
        }
    }
    
    public class DictionaryUShortKeyBinarySerializer : DictionaryBinarySerializer
    {
        public DictionaryUShortKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(ushort), ushort.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override object ReadKey(BinaryDataReader reader)
        {
            return reader.ReadUShort();
        }

        protected override void WriteKey(object key, BinaryDataWriter writer)
        {
            writer.WriteUShort((ushort) key);
        }
    }
    
    public class DictionaryIntKeyBinarySerializer : DictionaryBinarySerializer
    {
        public DictionaryIntKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(int), int.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override object ReadKey(BinaryDataReader reader)
        {
            return reader.ReadInt();
        }

        protected override void WriteKey(object key, BinaryDataWriter writer)
        {
            writer.WriteInt((int) key);
        }
    }
    
    public class DictionaryUIntKeyBinarySerializer : DictionaryBinarySerializer
    {
        public DictionaryUIntKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(int), uint.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override object ReadKey(BinaryDataReader reader)
        {
            return reader.ReadUInt();
        }

        protected override void WriteKey(object key, BinaryDataWriter writer)
        {
            writer.WriteUInt((uint) key);
        }
    }
    
    public class DictionaryLongKeyBinarySerializer : DictionaryBinarySerializer
    {
        public DictionaryLongKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(long), long.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override object ReadKey(BinaryDataReader reader)
        {
            return reader.ReadLong();
        }

        protected override void WriteKey(object key, BinaryDataWriter writer)
        {
            writer.WriteLong((long) key);
        }
    }
    
    public class DictionaryULongKeyBinarySerializer : DictionaryBinarySerializer
    {
        public DictionaryULongKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(ulong), ulong.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override object ReadKey(BinaryDataReader reader)
        {
            return reader.ReadULong();
        }

        protected override void WriteKey(object key, BinaryDataWriter writer)
        {
            writer.WriteULong((ulong) key);
        }
    }
}