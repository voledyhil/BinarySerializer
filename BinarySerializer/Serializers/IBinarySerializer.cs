using BinarySerializer.Data;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer.Serializers
{
    public interface IBinarySerializer
    {
        void Update(object obj, BinaryDataReader reader);
        void Serialize(object obj, BinaryDataWriter writer);
        void Serialize(object obj, BinaryDataWriter writer, IBaseline baseline);
    }

    public interface IBinarySerializer<TKey, TChildKey> : IBinarySerializer where TKey : unmanaged where TChildKey : unmanaged
    {
        void Serialize(object obj, BinaryDataWriter writer, IBaseline<TKey, TChildKey> baseline);
    }
}