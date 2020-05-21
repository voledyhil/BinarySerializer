using System.Collections.Generic;

namespace BinarySerializer.Serializers.Baselines
{
    public class PrimitiveListBaseline<T> : IBaseline where T : unmanaged 
    {
        public IEnumerable<T> Keys => _values;
        public int Count => _values.Count;

        private List<T> _values;

        private PrimitiveListBaseline(IEnumerable<T> values)
        {
            _values = new List<T>(values);
        }

        public PrimitiveListBaseline()
        {
            _values = new List<T>();
        }

        public bool Contains(T value)
        {
            return _values.Contains(value);
        }
       
        public void Add(T item)
        {
            _values.Add(item);
        }

        public void Remove(T item)
        {
            _values.Remove(item);
        }

        public IBaseline GetCopyOrNull()
        {
            return _values.Count > 0 ? new PrimitiveListBaseline<T>(_values) : null;
        }

        public void Dispose()
        {
            _values.Clear();
            _values = null;
        }
    }
}