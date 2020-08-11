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
            [BinaryItem(0)] public readonly ByteBinaryObjectCollection<ItemMock> ByteItems = new ByteBinaryObjectCollection<ItemMock>();
            [BinaryItem(1)] public readonly UShortBinaryObjectCollection<ItemMock> UShortItems = new UShortBinaryObjectCollection<ItemMock>();
            [BinaryItem(2)] public readonly ShortBinaryObjectCollection<ItemMock> ShortItems = new ShortBinaryObjectCollection<ItemMock>();
            [BinaryItem(3)] public readonly IntBinaryObjectCollection<ItemMock> IntItems = new IntBinaryObjectCollection<ItemMock>();
            [BinaryItem(4)] public readonly UIntBinaryObjectCollection<ItemMock> UIntItems = new UIntBinaryObjectCollection<ItemMock>();
            [BinaryItem(5)] public readonly LongBinaryObjectCollection<ItemMock> LongItems = new LongBinaryObjectCollection<ItemMock>();
            [BinaryItem(6)] public readonly ULongBinaryObjectCollection<ItemMock> ULongItems = new ULongBinaryObjectCollection<ItemMock>();
        }

        private class ItemMock
        {
            [BinaryItem(0)] public bool Bool;
            [BinaryItem(1)] public byte Byte;
            [BinaryItem(2)] public sbyte Sbyte;
            [BinaryItem(3)] public short Short;
            [BinaryItem(4)] public ushort UShort;
            [BinaryItem(5)] public int Int;
            [BinaryItem(6)] public uint UInt;
            [BinaryItem(7)] public long Long;
            [BinaryItem(8)] public ulong ULong;
            [BinaryItem(9)] public double Double;
            [BinaryItem(10)] public char Char;
            [BinaryItem(11)] public float Float;
            [BinaryItem(12, true)] public float ShortFloat;
            [BinaryItem(13)] public string String;
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