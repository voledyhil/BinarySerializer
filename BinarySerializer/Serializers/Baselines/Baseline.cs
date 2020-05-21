using System.Collections.Generic;

namespace BinarySerializer.Serializers.Baselines
{
    public class Baseline<TKey> : IBaseline where TKey : unmanaged
    {
        public IEnumerable<TKey> Keys => _values.Keys;
        public int Count => _values.Count;

        private Dictionary<TKey, IBaseline> _values = new Dictionary<TKey, IBaseline>();

        public Baseline()
        {
        }
        
        private Baseline(Dictionary<TKey, IBaseline> values)
        {
            foreach (KeyValuePair<TKey, IBaseline> item in values)
            {
                IBaseline value = item.Value.GetCopyOrNull();
                if (value != null)
                    _values.Add(item.Key, value);
            }
        }

        public IBaseline this[TKey key] => _values[key];

        public T GetOrCreateBaseline<T>(TKey key) where T : class, IBaseline, new()
        {
            if (_values.TryGetValue(key, out IBaseline item))
                return (T)item;
            
            T baseline = new T();
            _values.Add(key, baseline);
            return baseline;
        }

        public void DestroyBaseline(TKey key)
        {
            _values.Remove(key);
        }

        public IBaseline GetCopyOrNull()
        {
            return _values.Count > 0 ? new Baseline<TKey>(_values) : null;
        }

        public void Dispose()
        {
            _values.Clear();
            _values = null;
        }
    }
}