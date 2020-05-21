using System;
using System.Collections.Generic;
using BinarySerializer.Data;

namespace BinarySerializer.Serializers
{
    internal interface ICustomBinarySerializer : IBinarySerializer
    {
        
    }

    public abstract class CustomBinarySerializer<T> : ICustomBinarySerializer where T : class
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
            
        protected abstract void Update(T obj, BinaryDataReader reader);
        protected abstract void Serialize(T obj, BinaryDataWriter writer);
    }
}