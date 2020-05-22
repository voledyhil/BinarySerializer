using System;
using System.Collections.Generic;

namespace BinarySerializer.Serializers.Baselines
{
    public class Baseline : ICloneable, IDisposable
    {
        public IEnumerable<object> BaselineKeys => _baselines.Keys;
        
        private Dictionary<object, Baseline> _baselines;
        private Dictionary<object, object> _values;

        public Baseline()
        {
            _baselines = new Dictionary<object, Baseline>();
            _values = new Dictionary<object, object>();
        }
        
        private Baseline(IDictionary<object, Baseline> baselines, IDictionary<object, object> values)
        {
            _values = new Dictionary<object, object>(values);
            _baselines = new Dictionary<object, Baseline>();
            foreach (KeyValuePair<object, Baseline> item in baselines)
            {
                Baseline value = item.Value.Clone();
                if (value != null)
                    _baselines.Add(item.Key, value);
            }
        }

        public object this[object key]
        {
            get => _values[key];
            set => _values[key] = value;
        }

        public bool TryGetValue<T>(object key, out T value)
        {
            value = default;
            if (!_values.TryGetValue(key, out object objValue)) 
                return false;
            
            value = (T) objValue;
            return true;
        }
        
        public Baseline GetOrCreateBaseline(object key)
        {
            if (_baselines.TryGetValue(key, out Baseline item))
                return item;
            
            Baseline baseline = new Baseline();
            _baselines.Add(key, baseline);
            return baseline;
        }

        public void DestroyBaseline(object key)
        {
            _baselines.Remove(key);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
        
        public Baseline Clone()
        {
            return new Baseline(_baselines, _values);
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