using System;
using System.Collections.Generic;
using BinarySerializer.Data;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer.Serializers
{
    internal interface ICustomBinarySerializer : IBinarySerializer
    {

    }

    public abstract class CustomBinarySerializer<T> : IBinarySerializer<byte>, ICustomBinarySerializer where T : class
    {
        protected readonly IDictionary<Type, IBinarySerializer> Serializers;

        protected CustomBinarySerializer(IDictionary<Type, IBinarySerializer> serializers)
        {
            Serializers = serializers;
        }
        
        void IBinarySerializer.Update(object obj, BinaryDataReader reader)
        {
            Update((T) obj, reader);
        }

        void IBinarySerializer.Serialize(object obj, BinaryDataWriter writer)
        {
            Serialize((T) obj, writer);
        }

        void IBinarySerializer.Serialize(object obj, BinaryDataWriter writer, IBaseline baseline)
        {
            Serialize((T) obj, writer, (IBaseline<byte>)baseline);
        }
                
        void IBinarySerializer<byte>.Serialize(object obj, BinaryDataWriter writer, IBaseline<byte> baseline)
        {
            Serialize((T) obj, writer, baseline);
        }

        protected abstract void Update(T obj, BinaryDataReader reader);
        protected abstract void Serialize(T obj, BinaryDataWriter writer);
        protected abstract void Serialize(T obj, BinaryDataWriter writer, IBaseline<byte> baseline);
    }
}