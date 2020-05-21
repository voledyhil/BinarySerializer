using System.Collections.Generic;

namespace BinarySerializer.Serializers.Baselines
{
    public class PrimitiveDictionaryBaseline<TKey, TValue> : IBaseline
        where TKey : unmanaged where TValue : unmanaged
    {
        public IEnumerable<TKey> Keys => _values.Keys;
        public int Count => _values.Count;

        private Dictionary<TKey, TValue> _values;

        private PrimitiveDictionaryBaseline(IDictionary<TKey, TValue> values)
        {
            _values = new Dictionary<TKey, TValue>(values);
        }

        public PrimitiveDictionaryBaseline()
        {
            _values = new Dictionary<TKey, TValue>();
        }

        public TValue this[TKey key] => _values[key];

        public void Set(TKey key, TValue value)
        {
            _values[key] = value;
        }

        public void Remove(TKey key)
        {    
            _values.Remove(key);
        }

        public IBaseline GetCopyOrNull()
        {
            return _values.Count != 0 ? new PrimitiveDictionaryBaseline<TKey, TValue>(_values) : null;
        }

        public void Dispose()
        {
            _values.Clear();
            _values = null;
        }
    }
}