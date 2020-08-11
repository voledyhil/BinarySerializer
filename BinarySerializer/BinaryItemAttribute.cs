using System;

namespace BinarySerializer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class BinaryItemAttribute : Attribute
    {
        public byte Id { get; }
        public bool IsShort { get; }

        public BinaryItemAttribute(byte id)
        {
            Id = id;
        }

        public BinaryItemAttribute(byte id, bool isShort)
        {
            Id = id;
            IsShort = isShort;
        }
    }
}