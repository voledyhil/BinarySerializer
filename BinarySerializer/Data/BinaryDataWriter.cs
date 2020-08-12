using System;
using System.Text;

namespace BinarySerializer.Data
{
    public class BinaryDataWriter : BinaryBuffer
    {
        private BinaryDataWriter _parentWriter;
        private readonly int _startPos;
        
        public BinaryDataWriter()
        {
        }

        private BinaryDataWriter(BinaryDataWriter parentWriter, int offset) : base(parentWriter.Buffer, 0, 0)
        {
            _parentWriter = parentWriter;
            _startPos = _parentWriter.InnerLen + offset + sizeof(ushort);
            
            InnerLen = _startPos;
            Buffer.EnsureBufferSize(InnerLen);
        }
        
        public void WriteBool(bool value)
        {
            WriteByte(Convert.ToByte(value));
        }

        public void WriteByte(byte value)
        {
            Buffer.EnsureBufferSize(InnerLen + PrimitiveSize.ByteSize);
            Buffer[InnerLen] = value;
            IncLength(PrimitiveSize.ByteSize);
        }
        

        public void WriteShort(short value)
        {
            Buffer.EnsureBufferSize(InnerLen + PrimitiveSize.ShortSize);
            FastBitConverter.Write(Buffer.Data, InnerLen, value);
            IncLength(PrimitiveSize.ShortSize);
        }

        public void WriteSByte(sbyte value)
        {
            Buffer.EnsureBufferSize(InnerLen + PrimitiveSize.SByteSize);
            Buffer[InnerLen] = unchecked((byte) value);
            IncLength(PrimitiveSize.SByteSize);
        }

        public void WriteChar(char value)
        {
            Buffer.EnsureBufferSize(InnerLen + PrimitiveSize.CharSize);
            FastBitConverter.Write(Buffer.Data, InnerLen, value);
            IncLength(PrimitiveSize.CharSize);
        }
        
        public void WriteUShort(ushort value)
        {
            Buffer.EnsureBufferSize(InnerLen + PrimitiveSize.UShortSize);
            FastBitConverter.Write(Buffer.Data, InnerLen, value);
            IncLength(PrimitiveSize.UShortSize);
        }
        
        public void WriteDouble(double value)
        {
            Buffer.EnsureBufferSize(InnerLen + PrimitiveSize.DoubleSize);
            FastBitConverter.Write(Buffer.Data, InnerLen, value);
            IncLength(PrimitiveSize.DoubleSize);
        }

        public void WriteInt(int value)
        {
            Buffer.EnsureBufferSize(InnerLen + PrimitiveSize.IntSize);
            FastBitConverter.Write(Buffer.Data, InnerLen, value);
            IncLength(PrimitiveSize.IntSize);
        }

        public void WriteUInt(uint value)
        {
            Buffer.EnsureBufferSize(InnerLen + PrimitiveSize.UIntSize);
            FastBitConverter.Write(Buffer.Data, InnerLen, value);
            IncLength(PrimitiveSize.UIntSize);
        }

        public void WriteFloat(float value)
        {
            Buffer.EnsureBufferSize(InnerLen + PrimitiveSize.FloatSize);
            FastBitConverter.Write(Buffer.Data, InnerLen, value);
            IncLength(PrimitiveSize.FloatSize);
        }

        public void WriteLong(long value)
        {
            Buffer.EnsureBufferSize(InnerLen + PrimitiveSize.LongSize);
            FastBitConverter.Write(Buffer.Data, InnerLen, value);
            IncLength(PrimitiveSize.LongSize);
        }

        public void WriteULong(ulong value)
        {
            Buffer.EnsureBufferSize(InnerLen + PrimitiveSize.ULongSize);
            FastBitConverter.Write(Buffer.Data, InnerLen, value);
            IncLength(PrimitiveSize.ULongSize);
        }

        public void WriteShortFloat(float value)
        {
            WriteShort((short) (value * 256));
        }

        public void WriteString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                WriteUShort(0);
                return;
            }

            if (value.Length > ushort.MaxValue)
                value = value.Substring(0, ushort.MaxValue);

            byte[] bytes = Encoding.UTF8.GetBytes(value);
            WriteUShort((ushort) bytes.Length);
            Buffer.EnsureBufferSize(InnerLen + bytes.Length);
            Write(bytes);
        }

        public void CopyFrom(byte[] src, int srcOffset, int count)
        {
            Buffer.EnsureBufferSize(InnerLen + count);
            Buffer.CopyFrom(src, srcOffset, InnerLen, count);
            IncLength(count);
        }


        public BinaryDataWriter TryWriteNode(int offset)
        {
            return new BinaryDataWriter(this, offset);
        }

        public byte[] GetData()
        {
            byte[] bytes = new byte[Length];
            if (Length > 0)
                Buffer.CopyTo(bytes, _startPos, 0, Length);
            return bytes;
        }

        public void PushNode()
        {
            if (_parentWriter == null)
                return;
            
            if (Length > ushort.MaxValue)
                throw new ArgumentException($"node len '{Length}'. maximum len {ushort.MaxValue}");

            _parentWriter.WriteUShort(Convert.ToUInt16(Length));
            _parentWriter.IncLength(Length);
            _parentWriter = null;
        }

        public override void Dispose()
        {
            PushNode();

            base.Dispose();
        }
    }
}