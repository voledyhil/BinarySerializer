using BinarySerializer.Serializers;
using BinarySerializer.Serializers.Baselines;
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
        }

        private class NodeB
        {
            [BinaryItem(1)] public int UIntValue;
        }
        
        [TestMethod]
        public void SerializeTest()
        {
            NodeA source = new NodeA {IntValue = 1, UIntValue = 2};
            byte[] data = BinarySerializer.Serialize(source);
            
            NodeB target = new NodeB();
            BinarySerializer.Deserialize(target, data);
           
            Assert.AreEqual(target.UIntValue, source.UIntValue);
        }
    }
    
    [TestClass]
    public class ComplexTest
    {
        private class NodeA
        {
            [BinaryItem(0)]
            public readonly ByteBinaryObjectCollection<NodeB> Objects = new ByteBinaryObjectCollection<NodeB>();
        }

        private class NodeB
        {
            [BinaryItem(1)]
            public readonly ByteBinaryObjectCollection<ChildMock> Objects = new ByteBinaryObjectCollection<ChildMock>();    
        }
        
        [TestMethod]
        public void SerializeTest()
        {
            NodeA source = new NodeA();
            NodeB child = new NodeB();
            ChildMock obj = new ChildMock {Bool = true, Int = int.MaxValue};
            child.Objects.Add(0, obj);
            source.Objects.Add(0, child);

            Baseline<byte> baseline = new Baseline<byte>();
            byte[] data = BinarySerializer.Serialize(source, baseline);
            
            Assert.AreEqual(true, obj.Bool);
            Assert.AreEqual(int.MaxValue, obj.Int);
            
            NodeA target = new NodeA();
            BinarySerializer.Deserialize(target, data);
            
            Assert.AreEqual(source.Objects[0].Objects[0], target.Objects[0].Objects[0]);
            
            data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(0, data.Length);
        }
        
        [TestMethod]
        public void SerializeEmptyTest()
        {
            byte[] data = BinarySerializer.Serialize(new PrimitivesMock());
            Assert.AreEqual(0, data.Length);
        }
    }
}