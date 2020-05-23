using System;
using System.Reflection;
using BinarySerializer.Data;
using BinarySerializer.Expressions;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer.Serializers
{
    public abstract class WrapperBinarySerializer<TKey, TChildKey> : IBinarySerializer<TKey, TChildKey> where TKey : unmanaged where TChildKey : unmanaged
    {
        private readonly TKey _index;
        private readonly Getter<object> _getter;
        private readonly IBinarySerializer _serializer;

        protected WrapperBinarySerializer(TKey index, Type ownerType, FieldInfo field, IBinarySerializer serializer)
        {
            _index = index;
            _serializer = serializer;
            _getter = new Getter<object>(ownerType, field);
        }

        protected abstract void WriteIndex(TKey index, BinaryDataWriter writer);

        public void Update(object obj, BinaryDataReader reader)
        {
            using (BinaryDataReader childReader = reader.ReadNode())
            {
                _serializer.Update(_getter.Get(obj), childReader);
            }   
        }

        public void Serialize(object obj, BinaryDataWriter writer)
        {
            BinaryDataWriter childWriter = writer.TryWriteNode(sizeof(byte));
            _serializer.Serialize(_getter.Get(obj), childWriter);
            
            if (childWriter.Length <= 0)
                return;

            WriteIndex(_index, writer);
            childWriter.PushNode();
        }

        void IBinarySerializer.Serialize(object obj, BinaryDataWriter writer, IBaseline baseline)
        {
            Serialize(obj, writer, (IBaseline<TKey, TChildKey>)baseline);
        }

        public void Serialize(object obj, BinaryDataWriter writer, IBaseline<TKey, TChildKey> baseline)
        {
            BinaryDataWriter childWriter = writer.TryWriteNode(sizeof(byte));
            _serializer.Serialize(_getter.Get(obj), childWriter, baseline.GetOrCreateBaseline<TChildKey>(_index));
            
            if (childWriter.Length <= 0)
                return;
            
            WriteIndex(_index, writer);
            childWriter.PushNode();
        }
    }

    public class ByteWrapperBinarySerializer<TChildKey> : WrapperBinarySerializer<byte, TChildKey> where TChildKey : unmanaged
    {
        public ByteWrapperBinarySerializer(byte index, Type ownerType, FieldInfo field, IBinarySerializer serializer) : base(index, ownerType, field, serializer)
        {
        }

        protected override void WriteIndex(byte index, BinaryDataWriter writer)
        {
            writer.WriteByte(index);
        }
    }
}