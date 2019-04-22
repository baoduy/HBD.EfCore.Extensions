using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using HBD.Framework.Extensions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("HBD.EfCore.Hooks.Tests")]
namespace HBD.EfCore.Hooks.Triggers
{
    public static class TriggerExtensions
    {
        internal static IEnumerable<Type> GetProfileTypes(Assembly[] fromAssemblies, Type interfaceType)
            => fromAssemblies.Extract().Class().NotAbstract().IsInstanceOf(interfaceType).Distinct();

        /// <summary>
        /// Passing the Service Provider to Trigger Context
        /// </summary>
        /// <param name="service"></param>
        /// <param name="fromAssemblies">The assemblies that contain your <see cref="ITriggerProfile"/> </param>
        /// <returns></returns>
        internal static IServiceCollection UseTriggerProfiles(this IServiceCollection service, params Assembly[] fromAssemblies)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            if (TriggerHook.IsInitialized())
                return service;

            var interfaceType = typeof(ITriggerProfile);
            var profiles = GetProfileTypes(fromAssemblies, interfaceType);

            foreach (var p in profiles)
                service.Add(ServiceDescriptor.Singleton(interfaceType, p));

            TriggerHook.Initialize(service);

            return service;
        }

        /// <summary>
        /// Convert to Generic <see cref="TriggerEntityState"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static IEnumerable<TriggerEntityState<T>> ToTriggerEntity<T>(
            this IEnumerable<TriggerEntityState> entities)
            where T : class =>
            from entity in entities
            where entity.Entity is T
            select new TriggerEntityState<T>((T)entity.Entity, entity.ModifiedProperties, entity.State);

        private static readonly MethodInfo Method = typeof(TriggerExtensions)
            .GetMethod(nameof(ToTriggerEntity), BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Get Generic TriggerEntityState from collection based on EntityType
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        internal static IEnumerable<dynamic> GetTriggerEntity(this IEnumerable<TriggerEntityState> entities,
            Type entityType)
        {
            var result = Method.MakeGenericMethod(entityType).Invoke(null, new object[] { entities });
            return (IEnumerable<dynamic>)result;
        }

        /// <summary>
        /// Check whether the property has changed or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static bool HasChangedOn<T>(this TriggerEntityState<T> entity, Expression<Func<T, object>> selector) where T : class
        {
            var p = selector.GetPropertyAccess();
            return entity.ModifiedProperties.Any(i => i.EqualsIgnoreCase(p.Name));
        }
    }
}
