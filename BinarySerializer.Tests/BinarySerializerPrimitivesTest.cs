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
                Bool = true,                 //1. 2 byte file len + 1 byte data
                Byte = byte.MaxValue,        //2. 2 byte file len + 1 byte data
                Double = double.MaxValue,    //3. 2 byte file len + 8 byte data
                Int = int.MaxValue,          //4. 2 byte file len + 4 byte data
                Long = long.MaxValue,        //5. 2 byte file len + 8 byte data
                Sbyte = sbyte.MaxValue,      //6. 2 byte file len + 1 byte data
                Short = short.MaxValue,      //7. 2 byte file len + 2 byte data
                UInt = uint.MaxValue,        //8. 2 byte file len + 4 byte data
                ULong = ulong.MaxValue,      //9. 2 byte file len + 8 byte data
                UShort = ushort.MaxValue,    //10. 2 byte file len + 2 byte data
                Char = char.MaxValue,        //11. 2 byte file len + 2 byte data
                Float = float.MaxValue,      //12. 2 byte file len + 4 byte data
                ShortFloat = 1.5f            //13. 2 byte file len + 2 byte data
                // total 73 bytes
            };

            byte[] data = BinarySerializer.Serialize(source);

            Assert.AreEqual(86, data.Length); // fields data + indices data (73 + 13) 

            PrimitivesMock target = new PrimitivesMock();
            BinarySerializer.Deserialize(target, data);

            Assert.AreEqual(source, target);
        }

        [TestMethod]
        public void SerializePrimitivesBaselineTest()
        {
            PrimitivesMock source = new PrimitivesMock
            {
                Bool = true,                   //1. 2 byte file len + 1 byte
                Byte = byte.MaxValue - 1,      //2. 2 byte file len + 1 byte
                Double = double.MaxValue - 1,  //3. 2 byte file len + 8 byte
                Int = int.MaxValue - 1,        //4. 2 byte file len + 4 byte
                Long = long.MaxValue - 1,      //5. 2 byte file len + 8 byte
                Sbyte = sbyte.MaxValue - 1,    //6. 2 byte file len + 1 byte
                Short = short.MaxValue - 1,    //7. 2 byte file len + 2 byte
                UInt = uint.MaxValue - 1,      //8. 2 byte file len + 4 byte
                ULong = ulong.MaxValue - 1,    //9. 2 byte file len + 8 byte
                UShort = ushort.MaxValue - 1, //10. 2 byte file len + 2 byte
                Char = char.MaxValue,         //11. 2 byte file len + 2 byte
                Float = float.MaxValue - 1,   //12. 2 byte file len + 4 byte
                ShortFloat = 1.5f             //13. 2 byte file len + 2 byte
                // total 73 bytes
            };

            Baseline<byte> baseline = new Baseline<byte>();
            byte[] data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(86, data.Length); // fields data + indices data (73 + 13) 

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
            Assert.AreEqual(86, data.Length);
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);
        }
    }
}