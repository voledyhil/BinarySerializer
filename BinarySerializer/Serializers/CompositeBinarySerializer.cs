using System.Collections.Generic;
using BinarySerializer.Data;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer.Serializers
{
    public class CompositeBinarySerializer : IBinarySerializer
    {
        public int Count => _items.Count;
        private readonly Dictionary<byte, IBinarySerializer> _items;

        public CompositeBinarySerializer(Dictionary<byte, IBinarySerializer> items)
        {
            _items = items;
        }

        public void Update(object obj, BinaryDataReader reader)
        {
            while (reader.Position < reader.Length)
            {
                byte id = reader.ReadByte();
                if (_items.TryGetValue(id, out IBinarySerializer serializer))
                    serializer.Update(obj, reader);
            }
        }

        public void Serialize(object obj, BinaryDataWriter writer)
        {
            foreach (IBinarySerializer item in _items.Values)
            {
                item.Serialize(obj, writer);
            }
        }

        public void Serialize(object obj, BinaryDataWriter writer, IBaseline baseline)
        {
            foreach (IBinarySerializer item in _items.Values)
            {
                item.Serialize(obj, writer, baseline);
            }
        }
    }
}