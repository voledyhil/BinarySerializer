using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerializer.Tests
{
    [TestClass]
    public class BinarySerializerTest
    {
        [TestMethod]
        public void SerializeEmptyTest()
        {
            byte[] data = BinarySerializer.Serialize(new ParentMock());
            Assert.AreEqual(0, data.Length);
        }
        
        [TestMethod]
        public void SerializePrimitivesTest()
        {
            ParentMock source = new ParentMock
            {
                Bool = true,              //1.  1 byte
                Byte = byte.MaxValue,     //2.  1 byte
                Double = double.MaxValue, //3.  8 byte
                Int = int.MaxValue,       //4.  4 byte
                Long = long.MaxValue,     //5.  8 byte
                Sbyte = sbyte.MaxValue,   //6.  1 byte
                Short = short.MaxValue,   //7.  2 byte
                UInt = uint.MaxValue,     //8.  4 byte
                ULong = ulong.MaxValue,   //9.  8 byte
                UShort = ushort.MaxValue, //10. 2 byte
                Char = char.MaxValue,     //11. 2 byte
                Float = float.MaxValue,   //12. 4 byte
                ShortFloat = 1.5f         //13. 2 byte
                // total 47 bytes
            };

            byte[] data = BinarySerializer.Serialize(source);
            
            Assert.AreEqual(60, data.Length); // fields data + indices data (47 + 13) 
            
            ParentMock target = new ParentMock();
            BinarySerializer.Deserialize(target, data); 
            
            Assert.AreEqual(source, target);
        }


        [TestMethod]
        public void SerializeStringTest()
        {
            ParentMock source = new ParentMock {String = "DotNet"};
            byte[] data = BinarySerializer.Serialize(source);

            Assert.AreEqual(8, data.Length); // index + len + data (1 + 1 + 6) 

            ParentMock target = new ParentMock();
            BinarySerializer.Deserialize(target, data); 
            
            Assert.AreEqual(source, target);
        }
        
       
        [TestMethod]
        public void SerializeEnumTest()
        {
            ParentMock source = new ParentMock
            {
                ByteEnum = ByteEnum.Second, // 1 byte 
                IntEnum = IntEnum.Second // 4 byte
            };
            byte[] data = BinarySerializer.Serialize(source);

            Assert.AreEqual(7, data.Length); // fields data + indices data (5 + 2) 

            ParentMock target = new ParentMock();
            BinarySerializer.Deserialize(target, data); 
            
            Assert.AreEqual(source, target);
        }
        
        
        [TestMethod]
        public void SerializePropertiesTest()
        {
            ParentMock source = new ParentMock
            {
                BoolProperty = {Value = true},                 //1.  1 byte
                ByteProperty = {Value = byte.MaxValue},        //2.  1 byte
                DoubleProperty = {Value = double.MaxValue},    //3.  8 byte
                IntProperty = {Value = int.MaxValue},          //4.  4 byte
                LongProperty = {Value = long.MaxValue},        //5.  8 byte
                SbyteProperty = {Value = sbyte.MaxValue},      //6.  1 byte
                ShortProperty = {Value = short.MaxValue},      //7.  2 byte
                UIntProperty = {Value = uint.MaxValue},        //8.  4 byte
                ULongProperty = {Value = ulong.MaxValue},      //9.  8 byte
                UShortProperty = {Value = ushort.MaxValue},    //10. 2 byte
                CharProperty = {Value = char.MaxValue},        //11. 2 byte
                FloatProperty = {Value = float.MaxValue},      //12. 4 byte
                ShortFloatProperty = {Value = 1.5f}            //13. 2 byte
            };

            byte[] data = BinarySerializer.Serialize(source);
            
            Assert.AreEqual(60, data.Length); // fields data + indices data (47 + 13) 
            
            ParentMock target = new ParentMock();
            BinarySerializer.Deserialize(target, data); 
            
            Assert.AreEqual(source, target);
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
//        private enum EnumMock : byte
//        {
//            First,
//            Second
//        }
//        
//
//
//        private class ChildMock
//        {
//            public float FloatField
//            {
//                get => _floatField;
//                set => _floatField = value;
//            }
//
//            public float ShortFloatField
//            {
//                get => _shortFloatField;
//                set => _shortFloatField = value;
//            }
//
//            [BinaryItem] private float _floatField;
//            [BinaryItem(true)] private float _shortFloatField;  
//        }
//        
//
//        [TestMethod]
//        public void SerializeDeserializeTest()
//        {
//            ParentMock source = new ParentMock {IntField = 10, EnumField = EnumMock.Second};
//            source.ChildField.FloatField = 1.5f;
//            source.ChildField.ShortFloatField = 2.5f;
//            source.Dict.Add(10, new ChildMock { FloatField = 1, ShortFloatField = 10});
//            source.Dict.Add(20, new ChildMock { FloatField = 2, ShortFloatField = 20});
//
//            ParentMock target = new ParentMock();
//
//            byte[] data = BinarySerializer.Serialize(source);
//            BinarySerializer.Deserialize(target, data);
//            
//            Assert.AreEqual(source.EnumField, target.EnumField);
//            Assert.AreEqual(source.IntField, target.IntField);
//            Assert.AreEqual(source.StringField, target.StringField);
//            Assert.AreEqual(source.ChildField.FloatField, 1.5f);
//            Assert.AreEqual(source.ChildField.ShortFloatField, 2.5f);
//        }
    }
}