using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Stein.ViewModels.Tests
{
    public class Accessor<T>
    {
        private readonly Action<T> _setter;

        private readonly Func<T> _getter;

        public Accessor(Expression<Func<T>> expression)
        {
            var memberExpression = expression.Body as MemberExpression ?? throw new ArgumentException("Expression body is not of type MemberExpression", nameof(expression));
            var instanceExpression = memberExpression.Expression;
            var parameter = Expression.Parameter(typeof(T));

            if (memberExpression.Member is PropertyInfo propertyInfo)
            {
                _setter = Expression.Lambda<Action<T>>(Expression.Call(instanceExpression, propertyInfo.GetSetMethod(), parameter), parameter).Compile();
                _getter = Expression.Lambda<Func<T>>(Expression.Call(instanceExpression, propertyInfo.GetGetMethod())).Compile();
            }
            else if (memberExpression.Member is FieldInfo fieldInfo)
            {
                _setter = Expression.Lambda<Action<T>>(Expression.Assign(memberExpression, parameter), parameter).Compile();
                _getter = Expression.Lambda<Func<T>>(Expression.Field(instanceExpression, fieldInfo)).Compile();
            }
            else
            {
                throw new ArgumentException("Expression points to unsupported value.", nameof(expression));
            }
        }

        public T Value
        {
            get => _getter();
            set => _setter(value);
        }
    }
}
