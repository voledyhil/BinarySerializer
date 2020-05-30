using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BinarySerializer.Serializers;
using BinarySerializer.Serializers.Baselines;

namespace BinarySerializer.Benchmark
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class ComplexTest
    {
        private class CollectionsMock
        {
            [BinaryItem] public readonly ByteBinaryObjectCollection<ItemMock> ByteItems = new ByteBinaryObjectCollection<ItemMock>();
            [BinaryItem] public readonly UShortBinaryObjectCollection<ItemMock> UShortItems = new UShortBinaryObjectCollection<ItemMock>();
            [BinaryItem] public readonly ShortBinaryObjectCollection<ItemMock> ShortItems = new ShortBinaryObjectCollection<ItemMock>();
            [BinaryItem] public readonly IntBinaryObjectCollection<ItemMock> IntItems = new IntBinaryObjectCollection<ItemMock>();
            [BinaryItem] public readonly UIntBinaryObjectCollection<ItemMock> UIntItems = new UIntBinaryObjectCollection<ItemMock>();
            [BinaryItem] public readonly LongBinaryObjectCollection<ItemMock> LongItems = new LongBinaryObjectCollection<ItemMock>();
            [BinaryItem] public readonly ULongBinaryObjectCollection<ItemMock> ULongItems = new ULongBinaryObjectCollection<ItemMock>();
        }

        private class ItemMock
        {
            [BinaryItem] public bool Bool;
            [BinaryItem] public byte Byte;
            [BinaryItem] public sbyte Sbyte;
            [BinaryItem] public short Short;
            [BinaryItem] public ushort UShort;
            [BinaryItem] public int Int;
            [BinaryItem] public uint UInt;
            [BinaryItem] public long Long;
            [BinaryItem] public ulong ULong;
            [BinaryItem] public double Double;
            [BinaryItem] public char Char;
            [BinaryItem] public float Float;
            [BinaryItem(true)] public float ShortFloat;
            [BinaryItem] public string String;
        }

        private CollectionsMock _source;
        private byte[] _data;
        private Baseline<byte> _baseline;

        [GlobalSetup]
        public void Setup()
        {
            _source = new CollectionsMock();

            for (int i = 0; i < byte.MaxValue; i++)
            {
                _source.ByteItems.Add((byte) i, InstantiateItem());
                _source.UShortItems.Add((ushort) i, InstantiateItem());
                _source.ShortItems.Add((short) i, InstantiateItem());
                _source.IntItems.Add(i, InstantiateItem());
                _source.UIntItems.Add((uint) i, InstantiateItem());
                _source.LongItems.Add(i, InstantiateItem());
                _source.ULongItems.Add((ulong) i, InstantiateItem());
            }

            _baseline = new Baseline<byte>();
            _data = BinarySerializer.Serialize(_source, _baseline);
        }

        private static ItemMock InstantiateItem()
        {
            return new ItemMock
            {
                Bool = true,
                Byte = byte.MaxValue - 1,
                Double = double.MaxValue - 1,
                Int = int.MaxValue - 1,
                Long = long.MaxValue - 1,
                Sbyte = sbyte.MaxValue - 1,
                Short = short.MaxValue - 1,
                UInt = uint.MaxValue - 1,
                ULong = ulong.MaxValue - 1,
                UShort = ushort.MaxValue - 1,
                Char = char.MaxValue,
                Float = float.MaxValue - 1,
                ShortFloat = 1.5f,
                String = "DotNet"
            };
        }

        [Benchmark]
        public void SerializeTest()
        {
            byte[] data = BinarySerializer.Serialize(_source);
        }

        [Benchmark]
        public void SerializeEmptyBaselineTest()
        {
            Baseline<byte> baseline = new Baseline<byte>();
            byte[] data = BinarySerializer.Serialize(_source, baseline);
        }

        [Benchmark]
        public void SerializeFullBaselineTest()
        {
            byte[] data = BinarySerializer.Serialize(_source, _baseline);
        }

        [Benchmark]
        public void DeserializeTest()
        {
            CollectionsMock target = new CollectionsMock();
            BinarySerializer.Deserialize(target, _data);
        }
    }
}