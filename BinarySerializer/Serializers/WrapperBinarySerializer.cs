using System;
using System.Reflection;
using BinarySerializer.Data;
using BinarySerializer.Expressions;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer.Serializers
{
    public class WrapperBinarySerializer<TChildKey> : IBinarySerializer<byte, TChildKey> where TChildKey : unmanaged
    {
        private readonly byte _index;
        private readonly Getter<object> _getter;
        private readonly IBinarySerializer _serializer;

        public WrapperBinarySerializer(byte index, Type ownerType, FieldInfo field, IBinarySerializer serializer)
        {
            _index = index;
            _serializer = serializer;
            _getter = new Getter<object>(ownerType, field);
        }

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

            writer.WriteByte(_index);
            childWriter.PushNode();
        }

        void IBinarySerializer.Serialize(object obj, BinaryDataWriter writer, IBaseline baseline)
        {
            Serialize(obj, writer, (IBaseline<byte, byte>)baseline);
        }

        public void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, byte> baseline)
        {
            BinaryDataWriter childWriter = writer.TryWriteNode(sizeof(byte));
            _serializer.Serialize(_getter.Get(obj), childWriter, baseline.GetOrCreateBaseline<TChildKey>(_index));
            
            if (childWriter.Length <= 0)
                return;
            
            writer.WriteByte(_index);
            childWriter.PushNode();
        }
        
        public void Serialize(object obj, BinaryDataWriter writer, IBaseline<byte, TChildKey> baseline)
        {
            BinaryDataWriter childWriter = writer.TryWriteNode(sizeof(byte));
            _serializer.Serialize(_getter.Get(obj), childWriter, baseline.GetOrCreateBaseline<TChildKey>(_index));
            
            if (childWriter.Length <= 0)
                return;
            
            writer.WriteByte(_index);
            childWriter.PushNode();
        }
    }
}