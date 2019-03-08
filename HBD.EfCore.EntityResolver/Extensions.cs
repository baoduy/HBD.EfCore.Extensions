using HBD.EfCore.EntityResolver.Attributes;
using HBD.EfCore.EntityResolver.Internal;
using HBD.EfCore.Extensions;
using HBD.EfCore.Extensions.Specification;
using HBD.Framework.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

[assembly: InternalsVisibleTo("HBD.EfCore.EntityResolver.Tests")]

namespace HBD.EfCore.EntityResolver
{
    public static class Extensions
    {
        #region Public Methods
        /// <summary>
        /// Check whether the Type is instance of type
        ///   ex:   typeof(int).IsInstanceOf(typeof(IEquatable<>)).Should().BeTrue();
        ///         typeof(IEquatable<int>).IsInstanceOf(typeof(IEquatable<>)).Should().BeTrue();
        /// </summary>
        /// <param name="this"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInstanceOf(this Type @this, Type type)
        {
            if (type.IsGenericType && type.IsInterface)
            {
                if (@this.IsGenericType)
                    return @this.GetGenericTypeDefinition() == type;

                return @this.GetInterfaces().Any(y =>
                    y.IsGenericType && y.GetGenericTypeDefinition() == type || type.IsAssignableFrom(y));
            }

            return type.IsAssignableFrom(@this);
        }

        internal static IEnumerable<Type> GetGenericInterfaceTypes(this Type @this)
            => @this.GetInterfaces().Where(i => i.IsGenericType
                                                && !i.IsInstanceOf(typeof(IEquatable<>))
                                                && !i.IsInstanceOf(typeof(IComparable<>)));

        /// <summary>
        /// Convert Object to dynamic object which allow you can add more properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static dynamic ToDynamic<T>(this T @this) where T : class
        {
            IDictionary<string, object> d = new ExpandoObject();

            foreach (var p in @this.GetType().GetProperties().Where(p => p.CanRead))
                d.Add(p.Name, p.GetValue(@this));

            return (ExpandoObject)d;
        }

        #endregion Public Methods

        #region Internal Methods

        internal static IEnumerable<ResolverPropertyInfo> GetResolveInfo(Type modelType)
        {
            if (modelType == null) throw new ArgumentNullException(nameof(modelType));

            var info = modelType.GetProperties().Where(p => p.CanRead)
                  .Select(p => new
                  {
                      Property = p,
                      ResolveAttribute = p.GetCustomAttribute<AutoResolveAttribute>(),
                      IncludeAttribute = p.GetCustomAttribute<AlwaysIncludedAttribute>()
                  })
                  .Select(i => i.IncludeAttribute != null
                      ? new ResolverPropertyInfo(i.Property, i.IncludeAttribute)
                      : new ResolverPropertyInfo(i.Property, i.ResolveAttribute));

            foreach (var propertyInfo in info)
            {
                if (propertyInfo.AlwaysIncluded)
                {
                    yield return propertyInfo;
                    continue;
                }

                if (propertyInfo.Attribute == null) continue;

                //Tr to find the type from generic interface
                if (propertyInfo.Attribute.EntityType == null)
                {
                    var genericType = propertyInfo.Property.PropertyType.GetGenericInterfaceTypes().LastOrDefault();
                    if (genericType == null) throw new ArgumentException($"Cannot find the Entity type for property {propertyInfo.Property.Name}");

                    propertyInfo.Attribute.EntityType = genericType.GenericTypeArguments.First();
                }

                yield return propertyInfo;
            }
        }

        /// <summary>
        /// Get value for a Spec or by primary keys.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="keys"></param>
        /// <param name="specType"></param>
        /// <returns></returns>
        internal static async Task<IEnumerable<dynamic>> GetEntitiesByKeyOrSpecAsync<T>(
            DbContext dbContext,
            object keys,
            Type specType = null)
            where T : class
        {
            if (specType != null)
            {
                try
                {
                    var spec = Activator.CreateInstance(specType, keys) as Spec<T>;
                    return await dbContext.ForSpec(spec).ToListAsync();
                }
                catch (MissingMethodException ex)
                {
                    throw new MissingMethodException($"There is no constructor found for {keys.GetType().Name}", ex);
                }
            }

            return await Task.WhenAll(dbContext.GetKeys<T>(keys)
                .Select(k => dbContext.Set<T>().FindAsync(k)));
        }

        /// <summary>
        /// Get value for a Spec or by primary keys.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="keys"></param>
        /// <param name="specType"></param>
        /// <returns></returns>
        internal static IEnumerable<dynamic> GetEntitiesByKeyOrSpec<T>(
            DbContext dbContext,
            object keys,
            Type specType = null)
            where T : class
        {
            if (specType != null)
            {
                var spec = Activator.CreateInstance(specType, keys) as Spec<T>;
                return dbContext.ForSpec(spec).ToList();
            }

            return dbContext.GetKeys<T>(keys)
                .Select(k => dbContext.Set<T>().Find(k));
        }

        #endregion Internal Methods

        #region Private Methods
        /// <summary>
        /// Get primary keys for a T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static IEnumerable<object[]> GetKeys<T>(this DbContext dbContext, object values)
        {
            if (values.IsStringOrValueType())
                yield return new[] { values };
            else if (values is ICollection keys)
            {
                if (keys.Count <= 0)
                    yield break;

                var keyArray = keys.OfType<object>().ToArray();

                //Composite keys
                if (keyArray.All(i => i.IsStringOrValueType()))
                    yield return keyArray;

                //If it is array of Entity Class
                foreach (var key in keys)
                    yield return dbContext.GetKeyValuesOf<T>(key).ToArray();
            }
            //If it is a Model or Entity class
            else yield return dbContext.GetKeyValuesOf<T>(values).ToArray();
        }

        #endregion Private Methods
    }
}