using System;

namespace BinarySerializer.Data
{
    public class BinaryDataReader : BinaryBuffer
    {
        public int Position { get; private set; }

        private int _innerPosition;
        public BinaryDataReader(byte[] source, int offset = 0) : base(source)
        {
            _innerPosition = Position = offset;
            
            if (source != null)
                InnerLen = Length = source.Length;
        }

        private BinaryDataReader(BufferData bufferData, int innerPosition, int length, int innerLen) : base(bufferData, length, innerLen)
        {
            _innerPosition = innerPosition;
        }

        public bool ReadBool()
        {
            return Convert.ToBoolean(ReadByte());
        }

        public byte ReadByte()
        {
            if (!(InnerLen - _innerPosition >= PrimitiveSize.ByteSize))
                throw new BufferException("Trying to read past the buffer size");
            byte retval = Buffer[_innerPosition];
            _innerPosition += PrimitiveSize.ByteSize;
            Position += PrimitiveSize.ByteSize;
            return retval;
        }
        
        public char ReadChar()
        {
            if (!(InnerLen - _innerPosition >= PrimitiveSize.CharSize))
                throw new BufferException("Trying to read past the buffer size");
            char retval = Buffer.ToChar(_innerPosition);
            _innerPosition += PrimitiveSize.CharSize;
            Position += PrimitiveSize.CharSize;
            return retval;    
        }

        public short ReadShort()
        {
            if (!(InnerLen - _innerPosition >= PrimitiveSize.ShortSize))
                throw new BufferException("Trying to read past the buffer size");
            short retval = Buffer.ToInt16(_innerPosition);
            _innerPosition += PrimitiveSize.ShortSize;
            Position += PrimitiveSize.ShortSize;
            return retval;
        }

        public sbyte ReadSByte()
        {
            if (!(InnerLen - _innerPosition >= PrimitiveSize.SByteSize))
                throw new BufferException("Trying to read past the buffer size");
            sbyte retval = unchecked((sbyte)Buffer[_innerPosition]);
            _innerPosition += PrimitiveSize.SByteSize;
            Position += PrimitiveSize.SByteSize;
            return retval;
        }

        public ushort ReadUShort()
        {
            if (!(InnerLen - _innerPosition >= PrimitiveSize.UShortSize))
                throw new BufferException("Trying to read past the buffer size");
            ushort retval = Buffer.ToUInt16(_innerPosition);
            _innerPosition += PrimitiveSize.UShortSize;
            Position += PrimitiveSize.UShortSize;
            return retval;
        }

        public double ReadDouble()
        {
            if (!(InnerLen - _innerPosition >= PrimitiveSize.DoubleSize))
                throw new BufferException("Trying to read past the buffer size");
            double retval = Buffer.ToDouble(_innerPosition);
            _innerPosition += PrimitiveSize.DoubleSize;
            Position += PrimitiveSize.DoubleSize;
            return retval;
        }

        public int ReadInt()
        {
            return (int) ReadUInt();
        }

        public uint ReadUInt()
        {
            if (!(InnerLen - _innerPosition >= PrimitiveSize.IntSize))
                throw new BufferException("Trying to read past the buffer size");
            uint retval = Buffer.ToUInt32(_innerPosition);
            _innerPosition += PrimitiveSize.IntSize;
            Position += PrimitiveSize.IntSize;
            return retval;
        }

        public void Skip(int length)
        {
            _innerPosition += length;
            Position += length;
        }
        
        public long ReadLong()
        {
            return (long) ReadULong();
        }

        public ulong ReadULong()
        {
            if (!(InnerLen - _innerPosition >= PrimitiveSize.ULongSize))
                throw new BufferException("Trying to read past the buffer size");
            ulong retval = Buffer.ToUInt64(_innerPosition);
            _innerPosition += PrimitiveSize.ULongSize;
            Position += PrimitiveSize.ULongSize;
            return retval;
        }

        public float ReadShortFloat()
        {
            if (!(InnerLen - _innerPosition >= PrimitiveSize.ShortSize))
                throw new BufferException("Trying to read past the buffer size");
            short retval = Buffer.ToInt16(_innerPosition);
            _innerPosition += PrimitiveSize.ShortSize;
            Position += PrimitiveSize.ShortSize;
            return retval / 256f;
        }

        public float ReadFloat()
        {
            if (!(InnerLen - _innerPosition >= PrimitiveSize.FloatSize))
                throw new BufferException("Trying to read past the buffer size");
            float retval = Buffer.ToSingle(_innerPosition);
            _innerPosition += PrimitiveSize.FloatSize;
            Position += PrimitiveSize.FloatSize;
            return retval;
        }

        public string ReadString()
        {
            ushort byteLen = ReadUShort();
            if (byteLen <= 0)
                return null;

            if (InnerLen - _innerPosition < byteLen)
                throw new BufferException("Trying to read past the buffer size");

            string retval = Buffer.ToString(_innerPosition, byteLen);
            _innerPosition += byteLen;
            Position += byteLen;
            return retval;
        }

        public void CopyTo(byte[] dst, int srcOffset, int dstOffset, int count)
        {
            Buffer.CopyTo(dst, srcOffset, dstOffset, count);
        }

        public byte[] GetRemainingBytes()
        {
            int len = Length - Position;
            byte[] outgoingData = new byte[len];
            Buffer.CopyTo(outgoingData, _innerPosition, 0, len);
            Position = Length;
            return outgoingData;
        }

        public BinaryDataReader ReadNode()
        {
            ushort byteLen = ReadUShort();            
            if (byteLen == 0)
                return new BinaryDataReader(Buffer, _innerPosition, 0, InnerLen);

            if (InnerLen - _innerPosition < byteLen)
                throw new BufferException("Trying to read past the buffer size");

            int pos = _innerPosition;
            
            _innerPosition += byteLen;
            Position += byteLen;
            
            return new BinaryDataReader(Buffer, pos, byteLen, InnerLen);
        }

        public override void Dispose()
        {
            base.Dispose();

            _innerPosition = 0;
            Position = 0;
        }
    }
}