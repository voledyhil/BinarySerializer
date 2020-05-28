using BinarySerializer.Serializers.Baselines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerializer.Tests
{
    [TestClass]
    public class BinarySerializerEnumTest
    {
        [TestMethod]
        public void SerializeEnumTest()
        {
            EnumsMock source = new EnumsMock
            {
                ByteEnum = ByteEnum.Second, // 1 byte 
                IntEnum = IntEnum.Second // 4 byte
            };
            byte[] data = BinarySerializer.Serialize(source);

            Assert.AreEqual(7, data.Length); // fields data + indices data (5 + 2) 

            EnumsMock target = new EnumsMock();
            BinarySerializer.Deserialize(target, data);

            Assert.AreEqual(source, target);
        }

        [TestMethod]
        public void SerializeEnumBaselineTest()
        {
            EnumsMock source = new EnumsMock
            {
                ByteEnum = ByteEnum.Second, // 1 byte 
                IntEnum = IntEnum.Second // 4 byte
            };

            Baseline<byte> baseline = new Baseline<byte>();
            byte[] data = BinarySerializer.Serialize(source, baseline);

            Assert.AreEqual(7, data.Length); // fields data + indices data (5 + 2) 

            EnumsMock target = new EnumsMock();
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);

            data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(0, data.Length);
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);

            source.ByteEnum = ByteEnum.First;
            source.IntEnum = IntEnum.First;
            data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(7, data.Length);
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);
        }

    }
}