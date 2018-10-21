using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions.Attributes;

namespace HBD.EntityFrameworkCore.Extensions
{
    public static class Extensions
    {
        internal static Type GetEntityType(Type entityMappingType)
            => entityMappingType.GetInterfaces().First(a => a.IsGenericType).GetGenericArguments().First();

        /// <summary>
        /// Update the values from obj.
        /// Those properties marked as [ReadOnly(true)] or 
        /// TODO: Should improve the performance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="obj"></param>
        /// <param name="ignoreNull"></param>
        /// <param name="bindingFlags"></param>
        public static T UpdateFrom<T>(this T @this, T obj, bool ignoreNull = false, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public) where T : class
        {
            foreach (var property in obj.GetType().GetProperties(bindingFlags))
            {
                var readOnly = property.GetCustomAttribute<ReadOnlyAttribute>();
                var ignored = property.GetCustomAttribute<IgnoreFromUpdate>();

                if (ignored!=null 
                    ||readOnly?.IsReadOnly == true
                    || !property.CanRead
                    || !property.CanWrite) continue;

                var val = property.GetValue(obj);
                if(ignoreNull && val == null)
                    continue;

                property.SetValue(@this, val);
            }

            return @this;
        }

        /// <summary>
        /// Check whether the entity is referring by others.
        /// TODO: Need to improve the check.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        //public static bool IsEntityReferenceByOthers<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        //{
        //    var entry = context.Entry(entity);

        //    return entry.Navigations.Any(navigation =>
        //        !navigation.Metadata.IsCollection()
        //        && navigation.Query().Any());
        //}
    }
}
