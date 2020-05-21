using System.Collections.Generic;
using BinarySerializer.Data;

namespace BinarySerializer.Serializers
{
    public class CompositeBinarySerializer : IBinarySerializer
    {
        private readonly List<IBinarySerializer> _items;

        public CompositeBinarySerializer(List<IBinarySerializer> items)
        {
            _items = items;
        }

        public void Update(object obj, BinaryDataReader reader)
        {
            while (reader.Position < reader.Length)
            {
                _items[reader.ReadByte()].Update(obj, reader);
            }
        }

        public void Serialize(object obj, BinaryDataWriter writer)
        {
            for (int i = 0; i < _items.Count && i < byte.MaxValue; i++)
            {
                _items[i].Serialize(obj, writer);
            }
        }
    }
}