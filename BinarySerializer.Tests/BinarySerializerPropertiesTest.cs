using BinarySerializer.Serializers.Baselines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerializer.Tests
{
    [TestClass]
    public class BinarySerializerPropertiesTest
    {
        [TestMethod]
        public void SerializePropertiesTest()
        {
            PropertiesMock source = new PropertiesMock
            {
                BoolProperty = {Value = true},                 //1. 2 byte file len + 1 byte
                ByteProperty = {Value = byte.MaxValue},        //2. 2 byte file len + 1 byte
                DoubleProperty = {Value = double.MaxValue},    //3. 2 byte file len + 8 byte
                IntProperty = {Value = int.MaxValue},          //4. 2 byte file len + 4 byte
                LongProperty = {Value = long.MaxValue},        //5. 2 byte file len + 8 byte
                SbyteProperty = {Value = sbyte.MaxValue},      //6. 2 byte file len + 1 byte
                ShortProperty = {Value = short.MaxValue},      //7. 2 byte file len + 2 byte
                UIntProperty = {Value = uint.MaxValue},        //8. 2 byte file len + 4 byte
                ULongProperty = {Value = ulong.MaxValue},      //9. 2 byte file len + 8 byte
                UShortProperty = {Value = ushort.MaxValue},   //10. 2 byte file len + 2 byte
                CharProperty = {Value = char.MaxValue},       //11. 2 byte file len + 2 byte
                FloatProperty = {Value = float.MaxValue},     //12. 2 byte file len + 4 byte
                ShortFloatProperty = {Value = 1.5f}           //13. 2 byte file len + 2 byte
            };

            byte[] data = BinarySerializer.Serialize(source);

            Assert.AreEqual(86, data.Length); // fields data + indices data (73 + 13) 

            PropertiesMock target = new PropertiesMock();
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);
        }

        [TestMethod]
        public void SerializePropertiesBaselineTest()
        {
            PropertiesMock source = new PropertiesMock
            {
                BoolProperty = {Value = true},                 //1. 2 byte file len + 1 byte
                ByteProperty = {Value = byte.MaxValue},        //2. 2 byte file len + 1 byte
                DoubleProperty = {Value = double.MaxValue},    //3. 2 byte file len + 8 byte
                IntProperty = {Value = int.MaxValue},          //4. 2 byte file len + 4 byte
                LongProperty = {Value = long.MaxValue},        //5. 2 byte file len + 8 byte
                SbyteProperty = {Value = sbyte.MaxValue},      //6. 2 byte file len + 1 byte
                ShortProperty = {Value = short.MaxValue},      //7. 2 byte file len + 2 byte
                UIntProperty = {Value = uint.MaxValue},        //8. 2 byte file len + 4 byte
                ULongProperty = {Value = ulong.MaxValue},      //9. 2 byte file len + 8 byte
                UShortProperty = {Value = ushort.MaxValue},   //10. 2 byte file len + 2 byte
                CharProperty = {Value = char.MaxValue},       //11. 2 byte file len + 2 byte
                FloatProperty = {Value = float.MaxValue},     //12. 2 byte file len + 4 byte
                ShortFloatProperty = {Value = 1.5f}           //13. 2 byte file len + 2 byte
            };

            Baseline<byte> baseline = new Baseline<byte>();
            byte[] data = BinarySerializer.Serialize(source, baseline);

            Assert.AreEqual(86, data.Length); // fields data + indices data (73 + 13) 

            PropertiesMock target = new PropertiesMock();
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);


            data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(0, data.Length);
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);

            source.BoolProperty.Value = false;
            source.ByteProperty.Value = byte.MinValue;
            source.DoubleProperty.Value = double.MinValue;
            source.IntProperty.Value = int.MinValue;
            source.LongProperty.Value = long.MinValue;
            source.SbyteProperty.Value = sbyte.MinValue;
            source.ShortProperty.Value = short.MinValue;
            source.UIntProperty.Value = uint.MinValue;
            source.ULongProperty.Value = ulong.MinValue;
            source.UShortProperty.Value = ushort.MinValue;
            source.CharProperty.Value = char.MinValue;
            source.FloatProperty.Value = float.MinValue;
            source.ShortFloatProperty.Value = -1.5f;

            data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(86, data.Length);
            BinarySerializer.Deserialize(target, data);
            Assert.AreEqual(source, target);
        }
    }
}