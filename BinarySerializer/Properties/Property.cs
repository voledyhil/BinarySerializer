namespace BinarySerializer.Properties
{
    public interface IProperty
    {
        
    }
    
    public interface IProperty<T> : IProperty
    {
        T Value { get; set; }
    }
    
    public class Property<T> : IProperty<T>
    {
        public bool IsDirty => _version > _lastVersion;

        private int _version;
        private int _lastVersion;
        public void Update(T value)
        {
            Value = value;
            _lastVersion = _version;
        }

        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                _version++;
            }
        }
    }
}