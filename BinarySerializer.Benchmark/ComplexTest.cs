using System;
using System.Linq.Expressions;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace BinarySerializer.Benchmark
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class ComplexTest
    {
        private TestClass _testObj;    
        private FieldFastCaller<byte> _fieldCaller;

        private enum Enum1 : byte
        {
            First = 0,
            Second = 1
        }
        
        private class TestClass
        {
            private Enum1 _enum = Enum1.First;
            private int IntField = 10;
        }

        [GlobalSetup]
        public void Setup()
        {
            _testObj = new TestClass();

            FieldInfo fieldInfo = _testObj.GetType().GetField("_enum", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _fieldCaller = new FieldFastCaller<byte>(_testObj.GetType(), fieldInfo);


        }

        [Params(1000000)] public int IterationsCount;
        

        
        [Benchmark]
        public void GetFieldExpression()
        {
            for (int i = 0; i < IterationsCount; i++)
            {
                byte value = _fieldCaller.Get(_testObj);
            }            
        }

        [Benchmark]
        public void SetFieldExpression()
        {
            for (int i = 0; i < IterationsCount; i++)
            {
                _fieldCaller.Set(_testObj, 1);
            }            
        }


        private class FieldFastCaller<T>
        {
            private readonly Func<object, T> _getter;
            private readonly Action<object, T> _setter;
            
            public FieldFastCaller(Type type, FieldInfo info)
            {
                ParameterExpression instance = Expression.Parameter(typeof(object), "obj");
                ParameterExpression argument = Expression.Parameter(typeof(T), "arg");
                UnaryExpression newInstance = Expression.Convert(instance, type);
                MemberExpression exp = Expression.Field(newInstance, info);
                BinaryExpression assignExp = Expression.Assign(exp, argument);
                _getter = (Func<object, T>) Expression.Lambda(exp, instance).Compile();
                _setter = Expression.Lambda<Action<object, T>>(assignExp, instance, argument).Compile();
            }

            public T Get(object owner)
            {
                return _getter(owner);
            }

            public void Set(object owner, T value)
            {
                _setter(owner, value);
            }
        }



        private static Func<object, int> IntGetter(Type type, PropertyInfo info)
        {
            ParameterExpression instance = Expression.Parameter(typeof(object), "obj");
            UnaryExpression newInstance = Expression.Convert(instance, type);
            MemberExpression exp = Expression.Property(newInstance, info);
            return (Func<object, int>) Expression.Lambda(exp, instance).Compile();
        }
        
        private static Action<object, int> IntSetter(Type type, PropertyInfo info)
        {
            ParameterExpression instance = Expression.Parameter(typeof(object), "obj");
            ParameterExpression argument = Expression.Parameter(typeof(int), "arg");
            UnaryExpression newInstance = Expression.Convert(instance, type);
            MemberExpression exp = Expression.Property(newInstance, info);
            BinaryExpression assignExp = Expression.Assign(exp, argument);
            return Expression.Lambda<Action<object, int>>(assignExp, instance, argument).Compile();
        }

    }
}