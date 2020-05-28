using BinarySerializer.Serializers.Baselines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerializer.Tests
{
    [TestClass]
    public class BinarySerializerPrimitivesTest
    {
        [TestMethod]
        public void SerializePrimitivesTest()
        {
            PrimitivesMock source = new PrimitivesMock
            {
                Bool = true, //1.  1 byte
                Byte = byte.MaxValue, //2.  1 byte
                Double = double.MaxValue, //3.  8 byte
                Int = int.MaxValue, //4.  4 byte
                Long = long.MaxValue, //5.  8 byte
                Sbyte = sbyte.MaxValue, //6.  1 byte
                Short = short.MaxValue, //7.  2 byte
                UInt = uint.MaxValue, //8.  4 byte
                ULong = ulong.MaxValue, //9.  8 byte
                UShort = ushort.MaxValue, //10. 2 byte
                Char = char.MaxValue, //11. 2 byte
                Float = float.MaxValue, //12. 4 byte
                ShortFloat = 1.5f //13. 2 byte
                // total 47 bytes
            };

            byte[] data = BinarySerializer.Serialize(source);

            Assert.AreEqual(60, data.Length); // fields data + indices data (47 + 13) 

            PrimitivesMock target = new PrimitivesMock();
            BinarySerializer.Deserialize(target, data);

            Assert.AreEqual(source, target);
        }

        [TestMethod]
        public void SerializePrimitivesBaselineTest()
        {
            PrimitivesMock source = new PrimitivesMock
            {
                Bool = true,                   //1.  1 byte
                Byte = byte.MaxValue - 1,      //2.  1 byte
                Double = double.MaxValue - 1,  //3.  8 byte
                Int = int.MaxValue - 1,        //4.  4 byte
                Long = long.MaxValue - 1,      //5.  8 byte
                Sbyte = sbyte.MaxValue - 1,    //6.  1 byte
                Short = short.MaxValue - 1,    //7.  2 byte
                UInt = uint.MaxValue - 1,      //8.  4 byte
                ULong = ulong.MaxValue - 1,    //9.  8 byte
                UShort = ushort.MaxValue - 1, //10. 2 byte
                Char = char.MaxValue,         //11. 2 byte
                Float = float.MaxValue - 1,   //12. 4 byte
                ShortFloat = 1.5f             //13. 2 byte
                // total 47 bytes
            };

            Baseline<byte> baseline = new Baseline<byte>();
            byte[] data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(60, data.Length); // fields data + indices data (47 + 13) 

            PrimitivesMock target = new PrimitivesMock();
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);

            data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(0, data.Length);
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);

            source.Bool = false;
            source.Byte = byte.MinValue;
            source.Double = double.MinValue;
            source.Int = int.MinValue;
            source.Long = long.MinValue;
            source.Sbyte = sbyte.MinValue;
            source.Short = short.MinValue;
            source.UInt = uint.MinValue;
            source.ULong = ulong.MinValue;
            source.UShort = ushort.MinValue;
            source.Char = char.MinValue;
            source.Float = float.MinValue;
            source.ShortFloat = -1.5f;

            data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(60, data.Length);
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);
        }
    }
}