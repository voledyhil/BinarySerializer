using BinarySerializer.Serializers;
using BinarySerializer.Serializers.Baselines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerializer.Tests
{
    [TestClass]
    public class ComplexTest
    {
        public class NodeA
        {
            [BinaryItem]
            public readonly ByteBinaryObjectCollection<NodeB> Objects = new ByteBinaryObjectCollection<NodeB>();
        }
        
        public class NodeB
        {
            [BinaryItem]
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
    }
}