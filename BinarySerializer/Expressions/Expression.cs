using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BinarySerializer.Expressions
{
    public delegate object ObjectActivator();
    
    public static class Expression
    {
        public static ObjectActivator InstantiateCreator(ConstructorInfo ctor)
        {
            ParameterInfo[] paramsInfo = ctor.GetParameters();
            System.Linq.Expressions.Expression[] argsExp = new System.Linq.Expressions.Expression[paramsInfo.Length];
            NewExpression newExp = System.Linq.Expressions.Expression.New(ctor, argsExp);
            UnaryExpression conversion = System.Linq.Expressions.Expression.Convert(newExp, typeof(object));
            LambdaExpression lambda = System.Linq.Expressions.Expression.Lambda(typeof(ObjectActivator), conversion);
            return (ObjectActivator) lambda.Compile();
        }
        
        public static Func<object, T> InstantiateGetter<T>(Type ownerType, FieldInfo field)
        {
            ParameterExpression instance = System.Linq.Expressions.Expression.Parameter(typeof(object), "obj");
            UnaryExpression newInstance = System.Linq.Expressions.Expression.Convert(instance, ownerType);
            MemberExpression exp = System.Linq.Expressions.Expression.Field(newInstance, field);
            UnaryExpression conversion = System.Linq.Expressions.Expression.Convert(exp, typeof(T));
            return (Func<object, T>) System.Linq.Expressions.Expression.Lambda(conversion, instance).Compile();
        }
        
        public static Action<object, T> InstantiateSetter<T>(Type ownerType, FieldInfo field)
        {
            ParameterExpression instance = System.Linq.Expressions.Expression.Parameter(typeof(object), "obj");
            ParameterExpression argument = System.Linq.Expressions.Expression.Parameter(typeof(T), "arg");
            UnaryExpression newInstance = System.Linq.Expressions.Expression.Convert(instance, ownerType);
            UnaryExpression newArgument = System.Linq.Expressions.Expression.Convert(argument, field.FieldType);
            MemberExpression exp = System.Linq.Expressions.Expression.Field(newInstance, field);
            BinaryExpression assignExp = System.Linq.Expressions.Expression.Assign(exp, newArgument);
            return System.Linq.Expressions.Expression.Lambda<Action<object, T>>(assignExp, instance, argument).Compile();
        }
    }
}