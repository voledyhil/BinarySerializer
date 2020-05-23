using System;
using System.Collections;
using System.Collections.Generic;
using BinarySerializer.Data;
using BinarySerializer.Expressions;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer.Serializers
{
    internal interface IBinaryObjectCollection : IDictionary
    {
        
    }
    
    internal interface IBinaryObjectCollection<TKey> : IBinaryObjectCollection where TKey : unmanaged
    {
        new IEnumerable<TKey> Keys { get; }
        bool TryGetValue(TKey key, out object value);
    }

    internal interface IBinaryObjectCollection<TKey, TValue> : IDictionary<TKey, TValue>, IBinaryObjectCollection<TKey> where TKey : unmanaged where TValue : class, new()
    {
        
    }
    
    public class ByteBinaryObjectCollection<T> : Dictionary<byte, T>, IBinaryObjectCollection<byte, T> where T : class, new()
    {
        IEnumerable<byte> IBinaryObjectCollection<byte>.Keys => Keys;
        public bool TryGetValue(byte key, out object value)
        {
            if (TryGetValue(key, out T tValue))
            {
                value = tValue;
                return true;
            }
            value = null;    
            return false;
        }
    }

    public class ShortBinaryObjectCollection<T> : Dictionary<short, T>, IBinaryObjectCollection<short, T> where T : class, new()
    {
        IEnumerable<short> IBinaryObjectCollection<short>.Keys => Keys;
        public bool TryGetValue(short key, out object value)
        {
            if (TryGetValue(key, out T tValue))
            {
                value = tValue;
                return true;
            }
            value = null;    
            return false;
        }
    }
    
    public class UShortBinaryObjectCollection<T> : Dictionary<ushort, T>, IBinaryObjectCollection<ushort, T> where T : class, new()
    {
        IEnumerable<ushort> IBinaryObjectCollection<ushort>.Keys => Keys;
        public bool TryGetValue(ushort key, out object value)
        {
            if (TryGetValue(key, out T tValue))
            {
                value = tValue;
                return true;
            }
            value = null;    
            return false;
        }
    }
    
    public class IntBinaryObjectCollection<T> : Dictionary<int, T>, IBinaryObjectCollection<int, T> where T : class, new()
    {
        IEnumerable<int> IBinaryObjectCollection<int>.Keys => Keys;
        public bool TryGetValue(int key, out object value)
        {
            if (TryGetValue(key, out T tValue))
            {
                value = tValue;
                return true;
            }
            value = null;    
            return false;
        }
    }
    
    public class UIntBinaryObjectCollection<T> : Dictionary<uint, T>, IBinaryObjectCollection<uint, T> where T : class, new()
    {
        IEnumerable<uint> IBinaryObjectCollection<uint>.Keys => Keys;
        public bool TryGetValue(uint key, out object value)
        {
            if (TryGetValue(key, out T tValue))
            {
                value = tValue;
                return true;
            }
            value = null;    
            return false;
        }
    }
 
    public class LongBinaryObjectCollection<T> : Dictionary<long, T>, IBinaryObjectCollection<long, T> where T : class, new()
    {
        IEnumerable<long> IBinaryObjectCollection<long>.Keys => Keys;
        public bool TryGetValue(long key, out object value)
        {
            if (TryGetValue(key, out T tValue))
            {
                value = tValue;
                return true;
            }
            value = null;    
            return false;
        }
    }
    
    public class ULongBinaryObjectCollection<T> : Dictionary<ulong, T>, IBinaryObjectCollection<ulong, T> where T : class, new()
    {
        IEnumerable<ulong> IBinaryObjectCollection<ulong>.Keys => Keys;
        public bool TryGetValue(ulong key, out object value)
        {
            if (TryGetValue(key, out T tValue))
            {
                value = tValue;
                return true;
            }
            value = null;    
            return false;
        }
    }
    
    public abstract class DictionaryBinarySerializer<TKey> : IBinarySerializer where TKey : unmanaged
    {
        private readonly int _keySize;
        private readonly TKey _reservedKey;
        private readonly Creator _itemCreator;
        private readonly IBinarySerializer _itemSerializer;

        protected DictionaryBinarySerializer(int keySize, TKey reservedKey, Creator itemCreator,
            IBinarySerializer itemSerializer)
        {
            _keySize = keySize;
            _reservedKey = reservedKey;
            _itemCreator = itemCreator;
            _itemSerializer = itemSerializer;
        }

        protected abstract TKey ReadKey(BinaryDataReader reader);
        protected abstract void WriteKey(TKey key, BinaryDataWriter writer);

        public void Update(object obj, BinaryDataReader reader)
        {
            IBinaryObjectCollection<TKey> collection = (IBinaryObjectCollection<TKey>) obj;

            while (reader.Position < reader.Length)
            {
                TKey key = ReadKey(reader);
                if (_reservedKey.Equals(key))
                    break;

                using (BinaryDataReader itemReader = reader.ReadNode())
                {
                    if (!collection.TryGetValue(key, out object value))
                    {
                        value = _itemCreator.Create();
                        collection.Add(key, value);
                    }
                    _itemSerializer.Update(value, itemReader);
                }
            }

            while (reader.Position < reader.Length)
            {
                collection.Remove(ReadKey(reader));
            }
        }

        public void Serialize(object obj, BinaryDataWriter writer)
        {
            IBinaryObjectCollection<TKey> collection = (IBinaryObjectCollection<TKey>) obj;

            foreach (TKey key in collection.Keys)
            {
                if (_reservedKey.Equals(key))
                    throw new ArgumentException();
                
                BinaryDataWriter itemWriter = writer.TryWriteNode(_keySize);
                _itemSerializer.Serialize(collection[key], itemWriter);
                WriteKey(key, writer);
                itemWriter.PushNode();
            }
        }

        public void Serialize(object obj, BinaryDataWriter writer, IBaseline baseline)
        {
            IBaseline<TKey> tBaseline = (IBaseline<TKey>) baseline;
            IBinaryObjectCollection<TKey> collections = (IBinaryObjectCollection<TKey>) obj;

            List<TKey> baseKeys = new List<TKey>(tBaseline.BaselineKeys);
            foreach (TKey key in collections.Keys)
            {
                BinaryDataWriter itemWriter = writer.TryWriteNode(_keySize);
                
                _itemSerializer.Serialize(collections[key], itemWriter, tBaseline.GetOrCreateBaseline<Baseline<byte>>(key));

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
           
            foreach (TKey key in baseKeys)
            {
                WriteKey(key, writer);
                tBaseline.DestroyBaseline(key);
            }
        }
    }
    
    public class DictionaryByteKeyBinarySerializer : DictionaryBinarySerializer<byte>
    {
        public DictionaryByteKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(byte), byte.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override byte ReadKey(BinaryDataReader reader)
        {
            return reader.ReadByte();
        }

        protected override void WriteKey(byte key, BinaryDataWriter writer)
        {
            writer.WriteByte(key);
        }
    }

    public class DictionaryShortKeyBinarySerializer : DictionaryBinarySerializer<short>
    {
        public DictionaryShortKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(ushort), short.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override short ReadKey(BinaryDataReader reader)
        {
            return reader.ReadShort();
        }

        protected override void WriteKey(short key, BinaryDataWriter writer)
        {
            writer.WriteShort(key);
        }
    }
    
    public class DictionaryUShortKeyBinarySerializer : DictionaryBinarySerializer<ushort>
    {
        public DictionaryUShortKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(ushort), ushort.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override ushort ReadKey(BinaryDataReader reader)
        {
            return reader.ReadUShort();
        }

        protected override void WriteKey(ushort key, BinaryDataWriter writer)
        {
            writer.WriteUShort(key);
        }
    }
    
    public class DictionaryIntKeyBinarySerializer : DictionaryBinarySerializer<int>
    {
        public DictionaryIntKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(int), int.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override int ReadKey(BinaryDataReader reader)
        {
            return reader.ReadInt();
        }

        protected override void WriteKey(int key, BinaryDataWriter writer)
        {
            writer.WriteInt(key);
        }
    }
    
    public class DictionaryUIntKeyBinarySerializer : DictionaryBinarySerializer<uint>
    {
        public DictionaryUIntKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(int), uint.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override uint ReadKey(BinaryDataReader reader)
        {
            return reader.ReadUInt();
        }

        protected override void WriteKey(uint key, BinaryDataWriter writer)
        {
            writer.WriteUInt(key);
        }
    }
    
    public class DictionaryLongKeyBinarySerializer : DictionaryBinarySerializer<long>
    {
        public DictionaryLongKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(long), long.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override long ReadKey(BinaryDataReader reader)
        {
            return reader.ReadLong();
        }

        protected override void WriteKey(long key, BinaryDataWriter writer)
        {
            writer.WriteLong(key);
        }
    }
    
    public class DictionaryULongKeyBinarySerializer : DictionaryBinarySerializer<ulong>
    {
        public DictionaryULongKeyBinarySerializer(Creator itemCreator, IBinarySerializer itemSerializer) : base(
            sizeof(long), long.MaxValue, itemCreator, itemSerializer)
        {
        }

        protected override ulong ReadKey(BinaryDataReader reader)
        {
            return reader.ReadULong();
        }

        protected override void WriteKey(ulong key, BinaryDataWriter writer)
        {
            writer.WriteULong(key);
        }
    }
}