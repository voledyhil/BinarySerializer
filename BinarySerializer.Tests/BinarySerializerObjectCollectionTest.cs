using System.Linq;
using BinarySerializer.Serializers.Baselines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerializer.Tests
{
    [TestClass]
    public class BinarySerializerObjectCollectionTest
    {
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

        [TestMethod]
        public void SerializeObjectCollectionBaselineTest()
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

            Baseline<byte> baseline = new Baseline<byte>();
            byte[] data = BinarySerializer.Serialize(source, baseline);
            Assert.AreEqual(19 + 22 + 22 + 28 + 28 + 40 + 40, data.Length);

            CollectionsMock target = new CollectionsMock();
            BinarySerializer.Deserialize(target, data);
            
            Assert.AreEqual(3, baseline.GetOrCreateBaseline<Baseline<byte>>(0, 0, out _).BaselineKeys.ToArray().Length);
            Assert.AreEqual(3, baseline.GetOrCreateBaseline<Baseline<ushort>>(10, 0, out _).BaselineKeys.ToArray().Length);
            Assert.AreEqual(3, baseline.GetOrCreateBaseline<Baseline<short>>(20, 0, out _).BaselineKeys.ToArray().Length);
            Assert.AreEqual(3, baseline.GetOrCreateBaseline<Baseline<int>>(30, 0, out _).BaselineKeys.ToArray().Length);
            Assert.AreEqual(3, baseline.GetOrCreateBaseline<Baseline<uint>>(40, 0, out _).BaselineKeys.ToArray().Length);
            Assert.AreEqual(3, baseline.GetOrCreateBaseline<Baseline<long>>(50, 0, out _).BaselineKeys.ToArray().Length);
            Assert.AreEqual(3, baseline.GetOrCreateBaseline<Baseline<ulong>>(60, 0, out _).BaselineKeys.ToArray().Length);

            
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


            source.ByteObjects.Remove(0);
            source.ShortObjects.Remove(0);
            source.UShortObjects.Remove(0);
            source.IntObjects.Remove(0);
            source.UIntObjects.Remove(0);
            source.LongObjects.Remove(0);
            source.ULongObjects.Remove(0);
            
            data = BinarySerializer.Serialize(source, baseline);
            
            BinarySerializer.Deserialize(target, data);
            
            Assert.AreEqual(2, baseline.GetOrCreateBaseline<Baseline<byte>>(0, 0, out _).BaselineKeys.ToArray().Length);
            Assert.AreEqual(2, baseline.GetOrCreateBaseline<Baseline<ushort>>(10, 0, out _).BaselineKeys.ToArray().Length);
            Assert.AreEqual(2, baseline.GetOrCreateBaseline<Baseline<short>>(20, 0, out _).BaselineKeys.ToArray().Length);
            Assert.AreEqual(2, baseline.GetOrCreateBaseline<Baseline<int>>(30, 0, out _).BaselineKeys.ToArray().Length);
            Assert.AreEqual(2, baseline.GetOrCreateBaseline<Baseline<uint>>(40, 0, out _).BaselineKeys.ToArray().Length);
            Assert.AreEqual(2, baseline.GetOrCreateBaseline<Baseline<long>>(50, 0, out _).BaselineKeys.ToArray().Length);
            Assert.AreEqual(2, baseline.GetOrCreateBaseline<Baseline<ulong>>(60, 0, out _).BaselineKeys.ToArray().Length);
            
            
            Assert.AreEqual(2, target.ByteObjects.Count);
            Assert.AreEqual(source.ByteObjects[1], target.ByteObjects[1]);
            Assert.AreEqual(source.ByteObjects[3], target.ByteObjects[3]);

            Assert.AreEqual(2, target.ShortObjects.Count);
            Assert.AreEqual(source.ShortObjects[1], target.ShortObjects[1]);
            Assert.AreEqual(source.ShortObjects[3], target.ShortObjects[3]);

            Assert.AreEqual(2, target.UShortObjects.Count);
            Assert.AreEqual(source.UShortObjects[1], target.UShortObjects[1]);
            Assert.AreEqual(source.UShortObjects[3], target.UShortObjects[3]);

            Assert.AreEqual(2, target.IntObjects.Count);
            Assert.AreEqual(source.IntObjects[1], target.IntObjects[1]);
            Assert.AreEqual(source.IntObjects[3], target.IntObjects[3]);

            Assert.AreEqual(2, target.UIntObjects.Count);
            Assert.AreEqual(source.UIntObjects[1], target.UIntObjects[1]);
            Assert.AreEqual(source.UIntObjects[3], target.UIntObjects[3]);

            Assert.AreEqual(2, target.LongObjects.Count);
            Assert.AreEqual(source.LongObjects[1], target.LongObjects[1]);
            Assert.AreEqual(source.LongObjects[3], target.LongObjects[3]);

            Assert.AreEqual(2, target.ULongObjects.Count);
            Assert.AreEqual(source.ULongObjects[1], target.ULongObjects[1]);
            Assert.AreEqual(source.ULongObjects[3], target.ULongObjects[3]);
        }
    }
}