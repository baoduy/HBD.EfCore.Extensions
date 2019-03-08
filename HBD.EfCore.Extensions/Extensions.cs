using HBD.EfCore.Extensions.Attributes;
using HBD.EfCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HBD.EfCore.Extensions
{
    public static class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Get Primary Keys of a Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetKeys<TEntity>(this DbContext dbContext)
            => dbContext.GetKeys(typeof(TEntity));

        public static IEnumerable<string> GetKeys(this DbContext context, Type entityType)
                    => context.Model.FindEntityType(entityType)?.FindPrimaryKey().Properties.Select(i => i.Name)
                       ?? new string[0];

        /// <summary>
        /// Get Primary key value of a Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IEnumerable<object> GetKeyValuesOf<TEntity>(this DbContext context, object entity)
        {
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

        internal static void ConfigModelCreating(this DbContext dbContext,
                                            ModelBuilder modelBuilder)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            var options = dbContext.GetService<EntityMappingService>()?.EntityMapping;
            if (options == null) return;

            if (options.Registrations.Count <= 0)
                options.ScanFrom(dbContext.GetType().Assembly);

            //Register Entities
            modelBuilder.RegisterEntityMappingFrom(options.Registrations);
            //Register StaticData Of
            modelBuilder.RegisterStaticDataFrom(options.Registrations);
            //Register Data Seeding
            modelBuilder.RegisterDataSeedingFrom(options.Registrations);
        }

        internal static Type GetEntityType(Type entityMappingType)
                    => entityMappingType.GetInterfaces().First(a => a.IsGenericType).GetGenericArguments().First();

        #endregion Internal Methods
    }
}