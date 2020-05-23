using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerializer.Tests
{
    [TestClass]
    public class BinarySerializerTest
    {
        [TestMethod]
        public void SerializeEmptyTest()
        {
            byte[] data = BinarySerializer.Serialize(new PrimitivesMock());
            Assert.AreEqual(0, data.Length);
        }
        
        [TestMethod]
        public void SerializePrimitivesTest()
        {
            PrimitivesMock source = new PrimitivesMock
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
            
            PrimitivesMock target = new PrimitivesMock();
            BinarySerializer.Deserialize(target, data); 
            
            Assert.AreEqual(source, target);
        }


        [TestMethod]
        public void SerializeStringTest()
        {
            PrimitivesMock source = new PrimitivesMock {String = "DotNet"};
            byte[] data = BinarySerializer.Serialize(source);

            Assert.AreEqual(8, data.Length); // index + len + data (1 + 1 + 6) 

            PrimitivesMock target = new PrimitivesMock();
            BinarySerializer.Deserialize(target, data); 
            
            Assert.AreEqual(source, target);
        }
        
       
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
        public void SerializePropertiesTest()
        {
            PropertiesMock source = new PropertiesMock
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
            
            PropertiesMock target = new PropertiesMock();
            BinarySerializer.Deserialize(target, data); 
            
            Assert.AreEqual(source, target);
        }


        [TestMethod]
        public void SerializeObjectFieldTest()
        {
            ParentMock source = new ParentMock
            {
                ChildMock =
                {
                    Int = int.MaxValue,  // 4 byte
                    Bool = true          // 1 byte
                }
            };
            
            byte[] data = BinarySerializer.Serialize(source);
            
            Assert.AreEqual(10, data.Length); // parent index + child len + child indices + child data (1 + 2 + 2 + 5) 

            ParentMock target = new ParentMock();
            BinarySerializer.Deserialize(target, data); 
            
            Assert.AreEqual(source, target);
        }

        [TestMethod]
        public void SerializeObjectCollectionTest()
        {
            CollectionsMock source = new CollectionsMock();

            /*
                collection index             1 byte
                collection size              2 byte
                collection indices           3 * 1 byte
                    item 0 size              2 byte
                    item 0 int field index   1 byte
                    item 0 int field data    4 byte
                    
                    item 1 size              2 byte
                    item 1 bool field index  1 byte
                    item 1 bool field data   1 byte
                    
                    item 3 size              2 byte
                ------------------------------------
                                 19 byte
            */
            source.ByteObjects.Add(0, new ChildMock {Int = int.MaxValue});
            source.ByteObjects.Add(1, new ChildMock {Bool = true});
            source.ByteObjects.Add(3, new ChildMock());

            /*
                collection index             1 byte
                collection size              2 byte
                collection indices           3 * 2 byte
                    item 0 size              2 byte
                    item 0 int field index   1 byte
                    item 0 int field data    4 byte
                    
                    item 1 size              2 byte
                    item 1 bool field index  1 byte
                    item 1 bool field data   1 byte
                    
                    item 3 size              2 byte
                ------------------------------------
                                 22 byte
            */
            source.ShortObjects.Add(0, new ChildMock {Int = int.MaxValue});
            source.ShortObjects.Add(1, new ChildMock {Bool = true});
            source.ShortObjects.Add(3, new ChildMock());

            /*
                collection index             1 byte
                collection size              2 byte
                collection indices           3 * 2 byte
                    item 0 size              2 byte
                    item 0 int field index   1 byte
                    item 0 int field data    4 byte
                    
                    item 1 size              2 byte
                    item 1 bool field index  1 byte
                    item 1 bool field data   1 byte
                    
                    item 3 size              2 byte
                ------------------------------------
                                 22 byte
            */
            source.UShortObjects.Add(0, new ChildMock {Int = int.MaxValue});
            source.UShortObjects.Add(1, new ChildMock {Bool = true});
            source.UShortObjects.Add(3, new ChildMock());

            /*
                collection index             1 byte
                collection size              2 byte
                collection indices           3 * 4 byte
                    item 0 size              2 byte
                    item 0 int field index   1 byte
                    item 0 int field data    4 byte
                    
                    item 1 size              2 byte
                    item 1 bool field index  1 byte
                    item 1 bool field data   1 byte
                    
                    item 3 size              2 byte
                ------------------------------------
                                 28 byte
            */
            source.IntObjects.Add(0, new ChildMock {Int = int.MaxValue});
            source.IntObjects.Add(1, new ChildMock {Bool = true});
            source.IntObjects.Add(3, new ChildMock());

            /*
                collection index             1 byte
                collection size              2 byte
                collection indices           3 * 4 byte
                    item 0 size              2 byte
                    item 0 int field index   1 byte
                    item 0 int field data    4 byte
                    
                    item 1 size              2 byte
                    item 1 bool field index  1 byte
                    item 1 bool field data   1 byte
                    
                    item 3 size              2 byte
                ------------------------------------
                                 28 byte
            */
            source.UIntObjects.Add(0, new ChildMock {Int = int.MaxValue});
            source.UIntObjects.Add(1, new ChildMock {Bool = true});
            source.UIntObjects.Add(3, new ChildMock());

            /*
                collection index             1 byte
                collection size              2 byte
                collection indices           3 * 8 byte
                    item 0 size              2 byte
                    item 0 int field index   1 byte
                    item 0 int field data    4 byte
                    
                    item 1 size              2 byte
                    item 1 bool field index  1 byte
                    item 1 bool field data   1 byte
                    
                    item 3 size              2 byte
                ------------------------------------
                                 40 byte
            */
            source.LongObjects.Add(0, new ChildMock {Int = int.MaxValue});
            source.LongObjects.Add(1, new ChildMock {Bool = true});
            source.LongObjects.Add(3, new ChildMock());

            /*
                collection index             1 byte
                collection size              2 byte
                collection indices           3 * 8 byte
                    item 0 size              2 byte
                    item 0 int field index   1 byte
                    item 0 int field data    4 byte
                    
                    item 1 size              2 byte
                    item 1 bool field index  1 byte
                    item 1 bool field data   1 byte
                    
                    item 3 size              2 byte
                ------------------------------------
                                 40 byte
            */
            source.ULongObjects.Add(0, new ChildMock {Int = int.MaxValue});
            source.ULongObjects.Add(1, new ChildMock {Bool = true});
            source.ULongObjects.Add(3, new ChildMock());

            byte[] data = BinarySerializer.Serialize(source);
            Assert.AreEqual(19 + 22 + 22 + 28 + 28 + 40 + 40, data.Length);

            CollectionsMock target = new CollectionsMock();
            BinarySerializer.Deserialize(target, data);

            Assert.AreEqual(3, target.ByteObjects.Count);
            Assert.AreEqual(source.ByteObjects[0], target.ByteObjects[0]);
            Assert.AreEqual(source.ByteObjects[1], target.ByteObjects[1]);
            Assert.AreEqual(source.ByteObjects[3], target.ByteObjects[3]);

            Assert.AreEqual(3, target.ShortObjects.Count);
            Assert.AreEqual(source.ShortObjects[0], target.ShortObjects[0]);
            Assert.AreEqual(source.ShortObjects[1], target.ShortObjects[1]);
            Assert.AreEqual(source.ShortObjects[3], target.ShortObjects[3]);

            Assert.AreEqual(3, target.UShortObjects.Count);
            Assert.AreEqual(source.UShortObjects[0], target.UShortObjects[0]);
            Assert.AreEqual(source.UShortObjects[1], target.UShortObjects[1]);
            Assert.AreEqual(source.UShortObjects[3], target.UShortObjects[3]);

            Assert.AreEqual(3, target.IntObjects.Count);
            Assert.AreEqual(source.IntObjects[0], target.IntObjects[0]);
            Assert.AreEqual(source.IntObjects[1], target.IntObjects[1]);
            Assert.AreEqual(source.IntObjects[3], target.IntObjects[3]);

            Assert.AreEqual(3, target.UIntObjects.Count);
            Assert.AreEqual(source.UIntObjects[0], target.UIntObjects[0]);
            Assert.AreEqual(source.UIntObjects[1], target.UIntObjects[1]);
            Assert.AreEqual(source.UIntObjects[3], target.UIntObjects[3]);

            Assert.AreEqual(3, target.LongObjects.Count);
            Assert.AreEqual(source.LongObjects[0], target.LongObjects[0]);
            Assert.AreEqual(source.LongObjects[1], target.LongObjects[1]);
            Assert.AreEqual(source.LongObjects[3], target.LongObjects[3]);

            Assert.AreEqual(3, target.ULongObjects.Count);
            Assert.AreEqual(source.ULongObjects[0], target.ULongObjects[0]);
            Assert.AreEqual(source.ULongObjects[1], target.ULongObjects[1]);
            Assert.AreEqual(source.ULongObjects[3], target.ULongObjects[3]);
        }
    }
}