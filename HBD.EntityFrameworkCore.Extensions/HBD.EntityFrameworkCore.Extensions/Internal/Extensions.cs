using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions.Abstractions;

namespace HBD.EntityFrameworkCore.Extensions.Internal
{
    internal static class Extensions
    {
        public static TypeExtractor Extract(this Assembly assembly)
            => new TypeExtractor(assembly);

        public static TypeExtractor Extract(this Assembly[] assemblies)
            => new TypeExtractor(assemblies);

        public static ModelBuilder RegisterMapping<TEntity, TMapping>(this ModelBuilder builder)
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

        internal static Type GetEntityType(Type entityMappingType)
            => entityMappingType.GetInterfaces().First(a => a.IsGenericType).GetGenericArguments().First();

        internal static void RegisterMapping(this ModelBuilder modelBuilder, Type mapperType)
        {
            if (mapperType == null) throw new ArgumentNullException(nameof(mapperType));
            var method = typeof(Extensions).GetMethod(nameof(RegisterMapping));
            var eType = GetEntityType(mapperType);

            // ReSharper disable once PossibleNullReferenceException
            var md = method.MakeGenericMethod(eType, mapperType);
            md.Invoke(null, new object[] { modelBuilder });
        }

        internal static void RegisterMappingFromExtension(this ModelBuilder modelBuilder, EntityAutoMappingDbExtension extension)
        {
            var mappingTypes = GetEntityMappingTypes(extension.EntityAssemblies);
            var missingEntityTypes = GetEntityTypes(extension.EntityAssemblies, extension.Predicate).Where(t => mappingTypes.All(m => m.GetInterfaces().All(i => i.GenericTypeArguments.First() != t)));

            //Register mappingTypes
            foreach (var type in mappingTypes)
                modelBuilder.RegisterMapping(type);

            //Register missingEntityTypes
            foreach (var type in missingEntityTypes.Select(t => extension.DefaultEntityMapperType.MakeGenericType(t)))
                modelBuilder.RegisterMapping(type);
        }
    }
}
