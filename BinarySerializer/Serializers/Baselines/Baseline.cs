using System;
using System.Collections.Generic;

namespace BinarySerializer.Serializers.Baselines
{
    public interface IBaseline : ICloneable, IDisposable
    {
        new IBaseline Clone();
    }

    public interface IBaseline<TKey, TChildKey> : IBaseline where TKey : unmanaged where TChildKey : unmanaged
    {
        IEnumerable<TKey> BaselineKeys { get; }
        bool TryGetValue<TValue>(TKey key, out TValue value);
        IBaseline<TChildKey, TNewChildKey> GetOrCreateBaseline<TNewChildKey>(TKey key) where TNewChildKey : unmanaged;
        object this[TKey key] { get; set; }
        void DestroyBaseline(TKey key);
    }

    public class Baseline<TKey, TChild> : IBaseline<TKey, TChild> where TKey : unmanaged where TChild : unmanaged
    {
        public IEnumerable<TKey> BaselineKeys => _baselines.Keys;
        
        private Dictionary<TKey, IBaseline> _baselines;
        private Dictionary<TKey, object> _values;

        public Baseline()
        {
            _baselines = new Dictionary<TKey, IBaseline>();
            _values = new Dictionary<TKey, object>();
        }
        
        private Baseline(IDictionary<TKey, IBaseline> baselines, IDictionary<TKey, object> values)
        {
            _values = new Dictionary<TKey, object>(values);
            _baselines = new Dictionary<TKey, IBaseline>();
            foreach (KeyValuePair<TKey, IBaseline> item in baselines)
            {
                _baselines.Add(item.Key, item.Value.Clone());
            }
        }

        public object this[TKey key]
        {
            get => _values[key];
            set => _values[key] = value;
        }

        public bool TryGetValue<T>(TKey key, out T value)
        {
            value = default;
            if (!_values.TryGetValue(key, out object objValue)) 
                return false;
            
            value = (T) objValue;
            return true;
        }
        
        public IBaseline<TChild, TNewChildKey> GetOrCreateBaseline<TNewChildKey>(TKey key) where TNewChildKey : unmanaged
        {
            if (_baselines.TryGetValue(key, out IBaseline item))
                return (IBaseline<TChild, TNewChildKey>)item;
            
            Baseline<TChild, TNewChildKey> baseline = new Baseline<TChild, TNewChildKey>();
            _baselines.Add(key, baseline);
            return baseline;
        }

        public void DestroyBaseline(TKey key)
        {
            _baselines.Remove(key);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
        
        public IBaseline Clone()
        {
            return new Baseline<TKey, TChild>(_baselines, _values);
        }

        public void Dispose()
        {
            _baselines.Clear();
            _baselines = null;
            
            _values.Clear();
            _values = null;
        }
    }
}