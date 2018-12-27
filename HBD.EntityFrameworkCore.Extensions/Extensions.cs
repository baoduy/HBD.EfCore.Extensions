using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions.Attributes;
using Microsoft.EntityFrameworkCore;

namespace HBD.EntityFrameworkCore.Extensions
{
    public static class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Update the values from obj for Owned type ONLY. Those properties marked as
        /// [ReadOnly(true)] or
        /// TODO: Should improve the performance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="obj"></param>
        /// <param name="ignoreNull"></param>
        /// <param name="bindingFlags"></param>
        public static T UpdateFrom<T>(this T @this, T obj, bool ignoreNull = false,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public) where T : class
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (typeof(T).GetCustomAttribute<OwnedAttribute>() == null)
                throw new ArgumentException("Only Owned Type is accepted");

            foreach (var property in obj.GetType().GetProperties(bindingFlags))
            {
                var readOnly = property.GetCustomAttribute<ReadOnlyAttribute>();
                var ignored = property.GetCustomAttribute<IgnoreFromUpdateAttribute>();

                if (ignored != null
                    || readOnly?.IsReadOnly == true
                    || !property.CanRead
                    || !property.CanWrite) continue;

                var val = property.GetValue(obj);

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

        #endregion Public Methods

        #region Internal Methods

        internal static Type GetEntityType(Type entityMappingType)
                    => entityMappingType.GetInterfaces().First(a => a.IsGenericType).GetGenericArguments().First();

        #endregion Internal Methods

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