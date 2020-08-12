using BinarySerializer.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerializer.Tests
{
    [TestClass]
    public class MigrationTest
    {
        private class NodeA
        {
            [BinaryItem(0)] public int IntValue;
            [BinaryItem(1)] public int UIntValue;
            
            [BinaryItem(3)] public readonly ByteBinaryObjectCollection<ChildMock> Objects1 = new ByteBinaryObjectCollection<ChildMock>();
            [BinaryItem(4)] public readonly ByteBinaryObjectCollection<ChildMock> Objects2 = new ByteBinaryObjectCollection<ChildMock>();
        }

        private class NodeB
        {
            [BinaryItem(1)] public int UIntValue;
            [BinaryItem(4)] public readonly ByteBinaryObjectCollection<ChildMock> Objects2 = new ByteBinaryObjectCollection<ChildMock>();
        }

        [TestMethod]
        public void SerializeTest()
        {
            NodeA source = new NodeA {IntValue = 1, UIntValue = 2};
            source.Objects1.Add(0, new ChildMock {Int = 1});
            source.Objects2.Add(0, new ChildMock {Int = 1});

            byte[] data = BinarySerializer.Serialize(source);

            NodeB target = new NodeB();
            BinarySerializer.Deserialize(target, data);

            Assert.AreEqual(target.UIntValue, source.UIntValue);
            Assert.AreEqual(target.Objects2.Count, 1);
            Assert.AreEqual(target.Objects2[0].Int, 1);

        }
    }
}