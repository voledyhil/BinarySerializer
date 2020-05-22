using System;
using System.Collections.Generic;
using BinarySerializer.Data;
using BinarySerializer.Serializers.Baselines;

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

        void IBinarySerializer.Serialize(object obj, BinaryDataWriter writer, Baseline baseline)
        {
            Serialize((T) obj, writer, baseline);
        }

        protected abstract void Update(T obj, BinaryDataReader reader);
        protected abstract void Serialize(T obj, BinaryDataWriter writer);
        protected abstract void Serialize(T obj, BinaryDataWriter writer, Baseline baseline);
    }
}