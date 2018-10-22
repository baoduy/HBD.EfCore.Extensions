using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using HBD.EntityFrameworkCore.Extensions.Configurations;
using HBD.EntityFrameworkCore.Extensions.Options;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata.Internal;

namespace HBD.EntityFrameworkCore.Extensions
{
    public static class EntityTypeConfigurationExtensions
    {
        /// <summary>
        /// Generic RegisterMapping.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TMapping"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        internal static ModelBuilder RegisterMapping<TEntity, TMapping>(this ModelBuilder builder)
            where TMapping : IEntityTypeConfiguration<TEntity>
            where TEntity : class
        {
            var mapper = (IEntityTypeConfiguration<TEntity>)Activator.CreateInstance(typeof(TMapping));
            builder.ApplyConfiguration(mapper);
            return builder;
        }

        internal static Type[] GetEntityTypes(this Assembly[] assemblies, Expression<Func<Type, bool>> predicate = null)
            => assemblies.Extract().Class().NotAbstract().NotGeneric().NotInterface()
                .IsInstanceOf(typeof(IEntity<>)).Where(predicate).ToArray();

        internal static Type[] GetEntityMappingTypes(this Assembly[] assemblies)
            => assemblies.Extract().Class().NotAbstract().NotGeneric().NotInterface()
                .IsInstanceOf(typeof(IEntityTypeConfiguration<>)).ToArray();

        internal static void RegisterMappingFromType(this ModelBuilder modelBuilder, Type mapperType)
        {
            if (mapperType == null) throw new ArgumentNullException(nameof(mapperType));
            var method = typeof(EntityTypeConfigurationExtensions).GetMethod(nameof(RegisterMapping), BindingFlags.Static | BindingFlags.NonPublic);
            var eType = Extensions.GetEntityType(mapperType);

            if (method == null || eType == null)
                throw new ArgumentException($"The {nameof(RegisterMapping)} or EntityType are not found");

            var md = method.MakeGenericMethod(eType, mapperType);
            md.Invoke(null, new object[] { modelBuilder });
        }

        internal static Type GetGenericMapper(Type entityType, Type[] mapperTypes)
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

            throw new ArgumentException($"There is no {typeof(IEntityTypeConfiguration<>).Name} found for {entityType.Name}");
        }

        /// <summary>
        /// Scan and Load IEntityTypeConfiguration <see cref="IEntityTypeConfiguration{TEntity}"/> from Assemblies
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetMappers(this RegistrationInfo registration)
        {
            var genericMapperFromAssemblies = registration.EntityAssemblies.Extract().Generic().Class()
                .IsInstanceOf(typeof(IEntityTypeConfiguration<>)).ToArray();

            //There is no DefaultEntityMapperTypes then use the default one.
            if (registration.DefaultEntityMapperTypes == null || !registration.DefaultEntityMapperTypes.Any())
            {
                registration.WithDefaultMappersType(typeof(EntityTypeConfiguration<>));
            }

            var mappingTypes = registration.EntityAssemblies.GetEntityMappingTypes();
            var missingEntityTypes = registration.EntityAssemblies.GetEntityTypes(registration.Predicate)
                .Where(t => mappingTypes.All(m => m.GetInterfaces()
                    .All(i => i.GenericTypeArguments.First() != t)));

            return mappingTypes.Concat(missingEntityTypes.Select(t => GetGenericMapper(t, genericMapperFromAssemblies.Union(registration.DefaultEntityMapperTypes).ToArray())));
        }

        /// <summary>
        /// Register EntityTypeConfiguration from RegistrationInfos <see cref="RegistrationInfo"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="registrations"></param>
        public static void RegisterMappingFrom(this ModelBuilder modelBuilder, IEnumerable<RegistrationInfo> registrations)
        {
            foreach (var type in registrations.SelectMany(GetMappers))
                modelBuilder.RegisterMappingFromType(type);
        }
    }
}
