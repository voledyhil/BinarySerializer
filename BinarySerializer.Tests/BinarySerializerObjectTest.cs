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
                    Int = int.MaxValue, // 4 byte
                    Bool = true // 1 byte
                }
            };

            byte[] data = BinarySerializer.Serialize(source);

            Assert.AreEqual(10, data.Length); // parent index + child len + child indices + child data (1 + 2 + 2 + 5) 

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
                    Int = int.MaxValue, // 4 byte
                    Bool = true // 1 byte
                }
            };

            Baseline<byte> baseline = new Baseline<byte>();
            byte[] data = BinarySerializer.Serialize(source, baseline);

            Assert.AreEqual(10, data.Length); // parent index + child len + child indices + child data (1 + 2 + 2 + 5) 

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
            Assert.AreEqual(10, data.Length);
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);
        }


    }
}