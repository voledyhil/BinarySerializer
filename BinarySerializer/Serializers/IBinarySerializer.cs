using BinarySerializer.Data;

namespace BinarySerializer.Serializers
{
    public interface IBinarySerializer
    {
        void Update(object obj, BinaryDataReader reader);
        void Serialize(object obj, BinaryDataWriter writer);
    }
}