using HBD.EfCore.Extensions.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HBD.EfCore.Extensions
{
    public static class EfCoreExtensions
    {
        #region Methods

        /// <summary>
        /// Get Primary Keys of a Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetKeys<TEntity>(this DbContext dbContext)
            => dbContext.GetKeys(typeof(TEntity));

        public static IEnumerable<string> GetKeys(this DbContext context, Type entityType)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Model.FindEntityType(entityType)?.FindPrimaryKey().Properties.Select(i => i.Name)
                       ?? Array.Empty<string>();
        }

        /// <summary>
        /// Get Primary key value of a Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IEnumerable<object> GetKeyValuesOf<TEntity>(this DbContext context, object entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var keys = context.GetKeys<TEntity>();

            foreach (var key in keys)
                yield return entity.GetType().GetProperty(key)?.GetValue(entity);
        }

        /// <summary>
        /// Update the values from obj for Owned type ONLY. Those properties marked as
        /// [ReadOnly(true)] or
        /// TODO: Should improve the performance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="entity"></param>
        /// <param name="ignoreNull"></param>
        /// <param name="bindingFlags"></param>
        public static T UpdateFrom<T>(this T @this, T entity, bool ignoreNull = false,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public) where T : class
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (typeof(T).GetCustomAttribute<OwnedAttribute>() == null)
                throw new ArgumentException("Only Owned Type is accepted");

            foreach (var property in entity.GetType().GetProperties(bindingFlags))
            {
                var readOnly = property.GetCustomAttribute<ReadOnlyAttribute>();
                var ignored = property.GetCustomAttribute<IgnoreFromUpdateAttribute>();

                if (ignored != null
                    || readOnly?.IsReadOnly == true
                    || !property.CanRead
                    || !property.CanWrite) continue;

                var val = property.GetValue(entity);

                if (val == null)
                {
                    if (ignoreNull)
                        continue;

                    property.SetValue(@this, null);
                    continue;
                }

                property.SetValue(@this, val);
            }

            return @this;
        }

        internal static Type GetEntityType(Type entityMappingType)
                    => entityMappingType.GetInterfaces().First(a => a.IsGenericType).GetGenericArguments().First();

        #endregion Methods
    }
}