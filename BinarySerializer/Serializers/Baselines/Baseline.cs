using System;
using System.Collections.Generic;

namespace BinarySerializer.Serializers.Baselines
{
    public interface IBaseline : ICloneable, IDisposable
    {
        new IBaseline Clone();
    }

    public interface IBaseline<TKey> : IBaseline where TKey : unmanaged
    {
        IEnumerable<TKey> BaselineKeys { get; }
        bool TryGetValue<TValue>(TKey key, out TValue value);
        T GetOrCreateBaseline<T>(TKey key, out bool isNew) where T : class, IBaseline, new();
        object this[TKey key] { get; set; }
        void DestroyBaseline(TKey key);
    }

    public class Baseline<TKey> : IBaseline<TKey> where TKey : unmanaged
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
        
        public T GetOrCreateBaseline<T>(TKey key, out bool isNew) where T : class, IBaseline, new()
        {
            isNew = false;
            if (_baselines.TryGetValue(key, out IBaseline item))
                return (T)item;

            isNew = true;
            T baseline = new T();
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
            return new Baseline<TKey>(_baselines, _values);
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