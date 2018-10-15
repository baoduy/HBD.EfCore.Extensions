using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace HBD.EntityFrameworkCore.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Update the Not ReadOnly from obj.
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
                if (readOnly?.IsReadOnly == true
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
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool IsEntityInUse<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            var entry = context.Entry(entity);

            return entry.Navigations.Any(navigation =>
                !navigation.Metadata.IsCollection()
                && navigation.Query().Any());
        }
    }
}
