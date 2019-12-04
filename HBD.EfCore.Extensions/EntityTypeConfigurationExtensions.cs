using HBD.EfCore.Extensions.Abstractions;
using HBD.EfCore.Extensions.Attributes;
using HBD.EfCore.Extensions.Configurations;
using HBD.EfCore.Extensions.Options;
using HBD.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class EntityTypeConfigurationExtensions
    {
        #region Fields

        private static readonly MethodInfo Method = typeof(EntityTypeConfigurationExtensions)
                    .GetMethod(nameof(RegisterMapping), BindingFlags.Static | BindingFlags.NonPublic);

        #endregion Fields

        #region Methods

        /// <summary>
        /// Register EntityTypeConfiguration from RegistrationInfos <see cref="RegistrationInfo"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="registrations"></param>
        internal static void RegisterEntityMappingFrom(this ModelBuilder modelBuilder, IEnumerable<RegistrationInfo> registrations)
        {
            foreach (var type in registrations)
                modelBuilder.RegisterEntityMappingFrom(type);
        }

        private static Type CreateMapperFromGeneric(Type entityType, IEnumerable<Type> mapperTypes)
        {
            foreach (var mapperType in mapperTypes)
            {
                //The generic type should have 1 GenericTypeParameters only
                var gtype = mapperType.GetTypeInfo().GenericTypeParameters.Single();

                if (gtype.GetGenericParameterConstraints().Any(c => c.IsAssignableFrom(entityType))
                    || gtype.IsAssignableFrom(entityType)
                    || gtype.BaseType?.IsAssignableFrom(entityType) == true)
                    return mapperType.MakeGenericType(entityType);
            }

            throw new ArgumentException(
                $"There is no {typeof(IEntityTypeConfiguration<>).Name} found for {entityType.Name}");
        }

        /// <summary>
        /// Get all Class and Abstract class without Interface or Generic
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        private static IEnumerable<Type> GetAllEntityTypes(this RegistrationInfo registration)
                    => registration.EntityAssemblies.Extract().Class().NotGeneric().IsInstanceOf(typeof(IEntity<>))
                .Where(t => !t.HasAttribute<IgnoreMapperAttribute>(true));

        private static IEnumerable<Type> GetDefinedMappers(this RegistrationInfo registration)
                    => registration.EntityAssemblies.Extract().Class().NotAbstract().NotGeneric()
                        .IsInstanceOf(typeof(IEntityTypeConfiguration<>)).Distinct();

        private static IEnumerable<Type> GetGenericMappers(this RegistrationInfo registration)
                   => registration.EntityAssemblies.Extract().Generic().Class().NotAbstract()
                       .IsInstanceOf(typeof(IEntityTypeConfiguration<>)).Distinct();

        private static void RegisterEntityMappingFrom(this ModelBuilder modelBuilder, RegistrationInfo registration)
        {
            //modelBuilder.RegisterMappingFromType(type);
            if (registration.DefaultEntityMapperTypes == null)
                registration.WithDefaultMappersType(typeof(EntityTypeConfiguration<>));

            var allDefinedMappers = registration.GetDefinedMappers().ToList();
            var entityTypes = registration.GetAllEntityTypes().ToList();

            var requiredEntityTypes = registration.Predicate == null
                ? entityTypes
                : entityTypes.Where(registration.Predicate.Compile()).ToList();

            var generiMappers = registration.GetGenericMappers().Concat(registration.DefaultEntityMapperTypes).ToList();

            //Map Entities to ModelBuilder
            foreach (var entityType in requiredEntityTypes)
            {
                var mapper = allDefinedMappers.FirstOrDefault(m => m.BaseType?.GenericTypeArguments.FirstOrDefault() == entityType);

                if (mapper == null && !entityType.IsAbstract)
                    mapper = CreateMapperFromGeneric(entityType, generiMappers);

                if (mapper != null)
                    modelBuilder.RegisterMappingFromType(mapper);
            }

            //Ignore Others
            if (registration.IgnoreOtherEntities)
            {
                foreach (var entityType in entityTypes.Except(requiredEntityTypes))
                    modelBuilder.Ignore(entityType);
            }
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

            var eType = HBD.EfCore.Extensions.EfCoreExtensions.GetEntityType(mapperType);

            if (Method == null || eType == null)
                throw new ArgumentException($"The {nameof(RegisterMapping)} or EntityType are not found");

            var md = Method.MakeGenericMethod(eType, mapperType);
            md.Invoke(null, new object[] { modelBuilder });
        }

        #endregion Methods
    }
}