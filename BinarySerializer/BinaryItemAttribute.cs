using System;

namespace BinarySerializer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class BinaryItemAttribute : Attribute
    {
        public Type CustomSerializer { get; }
        public bool IsShort { get; }

        public BinaryItemAttribute()
        {
        }

        public BinaryItemAttribute(bool isShort)
        {
            IsShort = isShort;
        }

        public BinaryItemAttribute(Type customSerializer)
        {
            CustomSerializer = customSerializer;
        }
    }
}