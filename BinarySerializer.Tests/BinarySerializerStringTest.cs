using BinarySerializer.Serializers.Baselines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerializer.Tests
{
    [TestClass]
    public class BinarySerializerStringTest
    {
        [TestMethod]
        public void SerializeStringTest()
        {
            PrimitivesMock source = new PrimitivesMock {String = "DotNet"};
            byte[] data = BinarySerializer.Serialize(source);

            Assert.AreEqual(9, data.Length); // index + len + data (1 + 2 + 6) 

            PrimitivesMock target = new PrimitivesMock();
            BinarySerializer.Deserialize(target, data);

            Assert.AreEqual(source, target);
        }
        
        [TestMethod]
        public void SerializeStringBaselineTest()
        {
            PrimitivesMock source = new PrimitivesMock {String = "DotNet"};
            Baseline<byte> baseline = new Baseline<byte>();
            byte[] data = BinarySerializer.Serialize(source, baseline);

            Assert.AreEqual(9, data.Length); // index + len + data (1 + 2 + 6) 

            PrimitivesMock target = new PrimitivesMock();
            BinarySerializer.Deserialize(target, data);

            Assert.AreEqual(source, target);
            
            data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(0, data.Length);

            source.String = null; 
            data = BinarySerializer.Serialize(source, baseline);
            BinarySerializer.Deserialize(target, data);
            
            Assert.AreEqual(3, data.Length); // empty or null string
            Assert.AreEqual(source, target);
        }
        
        [TestMethod]
        public void SerializePropertyStringTest()
        {
            PropertiesMock source = new PropertiesMock {StringProperty = {Value = "DotNet"}};
            byte[] data = BinarySerializer.Serialize(source);

            Assert.AreEqual(9, data.Length); // index + len + data (1 + 2 + 6) 

            PropertiesMock target = new PropertiesMock();
            BinarySerializer.Deserialize(target, data);

            Assert.AreEqual(source, target);
        }
        
        
        [TestMethod]
        public void SerializePropertyBaselineStringTest()
        {
            PropertiesMock source = new PropertiesMock {StringProperty = {Value = "DotNet"}};
            Baseline<byte> baseline = new Baseline<byte>();
            byte[] data = BinarySerializer.Serialize(source, baseline);

            Assert.AreEqual(9, data.Length); // index + len + data (1 + 2 + 6) 

            PropertiesMock target = new PropertiesMock();
            BinarySerializer.Deserialize(target, data);

            Assert.AreEqual(source, target);
            
            data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(0, data.Length);

            source.StringProperty.Value = null; 
            data = BinarySerializer.Serialize(source, baseline);
            BinarySerializer.Deserialize(target, data);
            
            Assert.AreEqual(3, data.Length); // empty or null string
            Assert.AreEqual(source, target);
        }
    }
}