using HBD.EfCore.Extensions.Abstractions;
using HBD.EfCore.Extensions.Configurations;
using HBD.EfCore.Extensions.Options;
using HBD.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class EntityTypeConfigurationExtensions
    {
        #region Private Fields

        private static readonly MethodInfo Method = typeof(EntityTypeConfigurationExtensions)
                    .GetMethod(nameof(RegisterMapping), BindingFlags.Static | BindingFlags.NonPublic);

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Register EntityTypeConfiguration from RegistrationInfos <see cref="RegistrationInfo"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="registrations"></param>
        internal static void RegisterEntityMappingFrom(this ModelBuilder modelBuilder,
            IEnumerable<RegistrationInfo> registrations)
        {
            foreach (var type in registrations.SelectMany(GetMappers))
                modelBuilder.RegisterMappingFromType(type);
        }

        #endregion Public Methods

        #region Private Methods

        private static Type[] GetEntityMappingTypes(this Assembly[] assemblies)
            => assemblies.Extract().Class().NotAbstract().NotGeneric().NotInterface()
                .IsInstanceOf(typeof(IEntityTypeConfiguration<>)).ToArray();

        private static IEnumerable<Type> GetEntityTypes(this Assembly[] assemblies, Expression<Func<Type, bool>> predicate = null)
            => assemblies.Extract().Class().NotAbstract().NotGeneric().NotInterface()
                .IsInstanceOf(typeof(IEntity<>)).Where(predicate).ToArray();

        private static Type GetGenericMapper(Type entityType, IEnumerable<Type> mapperTypes)
        {
            foreach (var mapperType in mapperTypes)
            {
                //The generic type should have 1 GenericTypeParameters only
                var gtype = mapperType.GetTypeInfo().GenericTypeParameters.First();

                if (gtype.GetGenericParameterConstraints().Any(c => c.IsAssignableFrom(entityType))
                    || gtype.IsAssignableFrom(entityType)
                    || gtype.BaseType?.IsAssignableFrom(entityType) == true)
                    return mapperType.MakeGenericType(entityType);
            }

            throw new ArgumentException(
                $"There is no {typeof(IEntityTypeConfiguration<>).Name} found for {entityType.Name}");
        }

        /// <summary>
        /// Scan and Load IEntityTypeConfiguration <see cref="IEntityTypeConfiguration{TEntity}"/>
        /// from Assemblies
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Type> GetMappers(this RegistrationInfo registration)
        {
            var genericMapperFromAssemblies = registration.EntityAssemblies.Extract().Generic().Class()
                .IsInstanceOf(typeof(IEntityTypeConfiguration<>)).ToArray();

            //There is no DefaultEntityMapperTypes then use the default one.
            if (registration.DefaultEntityMapperTypes == null || !registration.DefaultEntityMapperTypes.Any())
                registration.WithDefaultMappersType(typeof(EntityTypeConfiguration<>));

            var mappingTypes = registration.EntityAssemblies.GetEntityMappingTypes();

            var missingEntityTypes = registration.EntityAssemblies.GetEntityTypes(registration.Predicate)
                .Where(t => mappingTypes.All(m => m.GetInterfaces()
                    .All(i => i.GenericTypeArguments.First() != t)));

            return mappingTypes.Concat(missingEntityTypes.Select(t =>
                GetGenericMapper(t,
                    genericMapperFromAssemblies.Union(registration.DefaultEntityMapperTypes).ToArray())));
        }

        /// <summary>
        /// Generic RegisterMapping.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TMapping"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static ModelBuilder RegisterMapping<TEntity, TMapping>(this ModelBuilder builder)
            where TMapping : IEntityTypeConfiguration<TEntity>
            where TEntity : class
        {
            var mapper = (IEntityTypeConfiguration<TEntity>)Activator.CreateInstance(typeof(TMapping));
            builder.ApplyConfiguration(mapper);
            return builder;
        }

        private static void RegisterMappingFromType(this ModelBuilder modelBuilder, Type mapperType)
        {
            if (mapperType == null) throw new ArgumentNullException(nameof(mapperType));

            var eType = HBD.EfCore.Extensions.Extensions.GetEntityType(mapperType);

            if (Method == null || eType == null)
                throw new ArgumentException($"The {nameof(RegisterMapping)} or EntityType are not found");

            var md = Method.MakeGenericMethod(eType, mapperType);
            md.Invoke(null, new object[] { modelBuilder });
        }

        #endregion Private Methods
    }
}