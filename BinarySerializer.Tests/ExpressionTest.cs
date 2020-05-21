using System;
using System.Collections;
using System.Collections.Generic;
using BinarySerializer.Data;
using BinarySerializer.Properties;
using BinarySerializer.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerializer.Tests
{
    [TestClass]
    public class BinarySerializerTest
    {
        private enum EnumMock : byte
        {
            First,
            Second
        }
        
        private class ParentMock
        {
            public int IntField
            {
                get => _intField.Value;
                set => _intField.Value = value;
            }

            public string StringField
            {
                get => _stringField;
                set => _stringField = value;
            }

            public EnumMock EnumField
            {
                get => _enumField;
                set => _enumField = value;
            }

            public Dictionary<ushort, ChildMock> Dict => _dict;

            public ChildMock ChildField => _childField;

            [BinaryItem] private EnumMock _enumField;
            [BinaryItem] private readonly Property<int> _intField = new Property<int>();
            [BinaryItem] private string _stringField;
            
            [BinaryItem(typeof(CustomChildMockSerializer))] 
            private readonly ChildMock _childField = new ChildMock();
            
            [BinaryItem]
            private readonly Dictionary<ushort, ChildMock> _dict = new Dictionary<ushort, ChildMock>();
        }

        private class ChildMock
        {
            public float FloatField
            {
                get => _floatField;
                set => _floatField = value;
            }

            public float ShortFloatField
            {
                get => _shortFloatField;
                set => _shortFloatField = value;
            }

            [BinaryItem] private float _floatField;
            [BinaryItem(true)] private float _shortFloatField;  
        }
        

        private class CustomChildMockSerializer : CustomBinarySerializer<ChildMock>
        {
            public CustomChildMockSerializer(IDictionary<Type, IBinarySerializer> binaryItems) : base(binaryItems)
            {
            }
            
            protected override void Update(ChildMock obj, BinaryDataReader reader)
            {
                while (reader.Position < reader.Length)
                {
                    byte index = reader.ReadByte();

                    if (index == 0)
                    {
                        obj.FloatField = reader.ReadFloat();
                    }
                    else
                    {
                        obj.ShortFloatField = reader.ReadShortFloat();
                    }
                }
            }

            protected override void Serialize(ChildMock obj, BinaryDataWriter writer)
            {
                writer.WriteByte(0);
                writer.WriteFloat(obj.FloatField);
                
                writer.WriteByte(1);
                writer.WriteShortFloat(obj.ShortFloatField);
            }
        }

        [TestMethod]
        public void SerializeDeserializeTest()
        {
            ParentMock source = new ParentMock {IntField = 10, EnumField = EnumMock.Second};
            source.ChildField.FloatField = 1.5f;
            source.ChildField.ShortFloatField = 2.5f;
            source.Dict.Add(10, new ChildMock { FloatField = 1, ShortFloatField = 10});
            source.Dict.Add(20, new ChildMock { FloatField = 2, ShortFloatField = 20});

            ParentMock target = new ParentMock();

            byte[] data = BinarySerializer.Serialize(source);
            BinarySerializer.Deserialize(target, data);
            
            Assert.AreEqual(source.EnumField, target.EnumField);
            Assert.AreEqual(source.IntField, target.IntField);
            Assert.AreEqual(source.StringField, target.StringField);
            Assert.AreEqual(source.ChildField.FloatField, 1.5f);
            Assert.AreEqual(source.ChildField.ShortFloatField, 2.5f);
        }
    }
}