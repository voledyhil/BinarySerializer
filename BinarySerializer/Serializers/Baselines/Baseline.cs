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
        bool TryGetValue(TKey key, out int value);
        T GetOrCreateBaseline<T>(TKey key, out bool isNew) where T : class, IBaseline, new();
        int this[TKey key] { get; set; }
        void DestroyBaseline(TKey key);
    }

    public class Baseline<TKey> : IBaseline<TKey> where TKey : unmanaged
    {
        public IEnumerable<TKey> BaselineKeys
        {
            get
            {
                if (_baselines == null)
                    _baselines = new Dictionary<TKey, IBaseline>();
                
                return _baselines.Keys;
            }
        }

        private Dictionary<TKey, IBaseline> _baselines;
        private Dictionary<TKey, int> _values;

        public Baseline()
        {
        }
        
        private Baseline(IDictionary<TKey, IBaseline> baselines, IDictionary<TKey, int> values)
        {
            if (values != null)
            {
                _values = new Dictionary<TKey, int>(values);
            }

            if (baselines != null)
            {
                _baselines = new Dictionary<TKey, IBaseline>();
                foreach (KeyValuePair<TKey, IBaseline> item in baselines)
                {
                    _baselines.Add(item.Key, item.Value.Clone());
                }
            }
        }

        public int this[TKey key]
        {
            get => _values[key];
            set
            {
                if (_values == null)
                    _values = new Dictionary<TKey, int>();
                
                _values[key] = value;
            }
        }

        public bool TryGetValue(TKey key, out int value)
        {
            if (_values == null)
                _values = new Dictionary<TKey, int>();

            value = default;
            return _values.TryGetValue(key, out value);
        }
        
        public T GetOrCreateBaseline<T>(TKey key, out bool isNew) where T : class, IBaseline, new()
        {
            if (_baselines == null)
                _baselines = new Dictionary<TKey, IBaseline>();
            
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
            _baselines?.Remove(key);
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
            if (_baselines != null)
            {
                _baselines.Clear();
                _baselines = null;
            }

            if (_values != null)
            {
                _values.Clear();
                _values = null;
            }
        }
    }
}