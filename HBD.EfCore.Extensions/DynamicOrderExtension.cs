using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HBD.EfCore.Extensions
{
    public static class DynamicOrderExtension
    {
        #region Methods

        public static IOrderedQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> source, string[] properties)
            =>
                properties.Aggregate<string, IOrderedQueryable<T>>(null,
                    (current, p) =>
                        current == null ? source.OrderByDescendingDynamic(p) : current.ThenByDescendingDynamic(p));

        public static IOrderedQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> source, string property)
            => ApplyOrder(source, property, "OrderByDescending");

        public static IOrderedQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string[] properties)
            =>
                properties.Aggregate<string, IOrderedQueryable<T>>(null,
                    (current, p) => current == null ? source.OrderByDynamic(p) : current.ThenByDynamic(p));

        public static IOrderedQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string property)
            => ApplyOrder(source, property, "OrderBy");

        public static IOrderedQueryable<T> ThenByDescendingDynamic<T>(this IOrderedQueryable<T> source, string property)
            => ApplyOrder(source, property, "ThenByDescending");

        public static IOrderedQueryable<T> ThenByDynamic<T>(this IOrderedQueryable<T> source, string property)
            => ApplyOrder(source, property, "ThenBy");

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));

            var props = property.Split('.');
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            //Get Property of Child Object and create expression OrderMethod(t=>t.Property)
            foreach (var prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = type.GetRuntimeProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            //Create generic order method
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetRuntimeMethods().First(
                    method => method.Name == methodName
                              && method.IsGenericMethodDefinition
                              && method.GetGenericArguments().Length == 2
                              && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<T>)result;
        }

        #endregion Methods
    }
}