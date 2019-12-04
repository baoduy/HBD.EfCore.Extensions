using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HBD.EfCore.Extensions
{
    public static class ExpressionExtensions
    {
        //private static readonly MethodInfo ContainsMethod = typeof(string).GetTypeInfo().GetMethod("Contains");

        #region Methods

        /// <summary>
        /// Combine two expressions with And
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var and = Expression.AndAlso(left.Body, Expression.Invoke(right, left.Parameters[0]));
            return Expression.Lambda<Func<T, bool>>(and, left.Parameters);
        }

        public static IEnumerable<PropertyInfo> ExtractProperties<T, TKey>(this Expression<Func<T, TKey>> @this)
            where T : class
        {
            if (@this == null) yield break;

            var queue = new Queue<Expression>();
            queue.Enqueue(@this.Body);

            while (queue.Count > 0)
            {
                var ex = queue.Dequeue();

                switch (ex)
                {
                    case MemberExpression expression:
                        {
                            dynamic tmp = expression;
                            yield return tmp.Member;
                            break;
                        }
                    case UnaryExpression expression:
                        {
                            dynamic tmp = expression.Operand as MemberExpression;
                            yield return tmp?.Member;
                            break;
                        }
                    case BinaryExpression expression:
                        {
                            var tmp = expression;
                            queue.Enqueue(tmp.Left);
                            queue.Enqueue(tmp.Right);
                            break;
                        }
                    case MethodCallExpression expression:
                        {
                            dynamic tmp = expression;
                            yield return tmp.Object.Member;
                            break;
                        }
                }
            }
        }

        public static PropertyInfo ExtractProperty<T, TKey>(this Expression<Func<T, TKey>> @this)
                            where T : class =>
            @this.ExtractProperties().SingleOrDefault();

        /// <summary>
        /// Get Not of the expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> NotMe<T>(this Expression<Func<T, bool>> @this)
        {
            var not = Expression.Not(@this.Body);
            return Expression.Lambda<Func<T, bool>>(not, @this.Parameters);
        }

        /// <summary>
        /// Combine two expressions with Or
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            return Expression.Or(left.Body, right.Body, left.Parameters.Single());
        }

        #endregion Methods

        //public static PropertyInfo ExtractProperty<T>(this Expression<Func<T,object>> propertyExpression)
        //{
        //    if (propertyExpression == null)
        //        throw new ArgumentNullException(nameof(propertyExpression));

        // if (!(propertyExpression.Body is MemberExpression memberExpression)) throw new NotSupportedException(propertyExpression.Body.GetType().FullName);

        // var property = memberExpression.Member as PropertyInfo; if (property == null) throw new NotSupportedException(memberExpression.Member.GetType().FullName);

        // var getMethod = property.GetMethod; if (getMethod.IsStatic) throw new
        // NotSupportedException("Static Method");

        //    return property;
        //}

        //public static Expression<Func<T, bool>> ToContainsExpress<T>(string value, params string[] propertyNames)
        //{
        //    if (propertyNames.NotAny()) return null;

        // var pe = Expression.Parameter(typeof(T));

        // dynamic expression = null; foreach (var p in propertyNames.Where(a =>
        // a.IsNotNullOrEmpty())) { var left = Expression.Property(pe, p); var right = Expression.Constant(value);

        // expression = expression == null ? Expression.Call(left, ContainsMethod, right) :
        // Expression.Or(expression, Expression.Call(left, ContainsMethod, right)); }

        //    return Expression.Lambda(expression, pe);
        //}

        //public static Expression<Func<T, bool>> ToEqualsExpress<T>(this T @this, params string[] propertyNames)
        //    where T : class
        //{
        //    if (@this == null) return null;
        //    if (propertyNames.NotAny()) return null;

        // var pe = Expression.Parameter(typeof(T));

        // dynamic expression = null; foreach (var p in propertyNames.Where(a =>
        // a.IsNotNullOrEmpty())) { var value = @this.PropertyValue(p); var left =
        // Expression.Property(pe, p); var right = Expression.Constant(value);

        // expression = expression == null ? Expression.Equal(left, right) :
        // Expression.And(expression, Expression.Equal(left, right)); }

        //    return Expression.Lambda(expression, pe);
        //}

        //public static Expression<Func<T, bool>> ToEqualsExpress<T>(this Dictionary<string, object> @this)
        //{
        //    if (@this.NotAny()) return null;

        // var pe = Expression.Parameter(typeof(T));

        // dynamic expression = null; foreach (var k in @this.Where(a => a.Key.IsNotNullOrEmpty())) {
        // var value = k.Value; var left = Expression.Property(pe, k.Key); var right = Expression.Constant(value);

        // expression = expression == null ? Expression.Equal(left, right) :
        // Expression.And(expression, Expression.Equal(left, right)); }

        //    return Expression.Lambda(expression, pe);
        //}
    }
}