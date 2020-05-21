using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BinarySerializer.Expressions
{
    public delegate object ObjectActivator(params object[] args);
    
    public class Creator
    {
        private readonly ObjectActivator _activator;
        public Creator(ConstructorInfo ctor)
        {
            ParameterInfo[] paramsInfo = ctor.GetParameters();
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");
            Expression[] argsExp = new Expression[paramsInfo.Length];

            for (int i = 0; i < paramsInfo.Length; i++)
            {
                ConstantExpression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;
                BinaryExpression paramAccessorExp = Expression.ArrayIndex(param, index);
                UnaryExpression paramCastExp = Expression.Convert(paramAccessorExp, paramType);
                argsExp[i] = paramCastExp;
            }
            
            NewExpression newExp = Expression.New(ctor, argsExp);
            UnaryExpression conversion = Expression.Convert(newExp, typeof(object));
            LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator), conversion, param);
            
            _activator = (ObjectActivator) lambda.Compile();
        }

        public object Create(params object[] args)
        {
            return _activator(args);
        }
    }
}