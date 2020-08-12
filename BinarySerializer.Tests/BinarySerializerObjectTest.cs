using BinarySerializer.Serializers.Baselines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerializer.Tests
{
    [TestClass]
    public class BinarySerializerObjectTest
    {
        [TestMethod]
        public void SerializeObjectTest()
        {
            ParentMock source = new ParentMock
            {
                ChildMock =
                {
                    Int = int.MaxValue,  // 2 byte field len, 4 byte data len
                    Bool = true          // 2 byte field len, 1 data byte
                }
            };

            byte[] data = BinarySerializer.Serialize(source);

            Assert.AreEqual(14, data.Length); // parent index + child len + child indices + child data + child field size (1 + 2 + (2 + 2) + (5 + 2)) 

            ParentMock target = new ParentMock();
            BinarySerializer.Deserialize(target, data);

            Assert.AreEqual(source, target);
        }

        [TestMethod]
        public void SerializeObjectBaselineTest()
        {
            ParentMock source = new ParentMock
            {
                ChildMock =
                {
                    Int = int.MaxValue, // 2 byte field len, 4 byte data len
                    Bool = true // 2 byte field len, 1 data byte
                }
            };

            Baseline<byte> baseline = new Baseline<byte>();
            byte[] data = BinarySerializer.Serialize(source, baseline);

            Assert.AreEqual(14, data.Length); // parent index + child len + child indices + child data + child field size (1 + 2 + (2 + 2) + (5 + 2)) 

            ParentMock target = new ParentMock();
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);

            data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(0, data.Length);
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);

            source.ChildMock.Int = int.MinValue;
            source.ChildMock.Bool = false;
            data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(14, data.Length);
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);
        }


    }
}