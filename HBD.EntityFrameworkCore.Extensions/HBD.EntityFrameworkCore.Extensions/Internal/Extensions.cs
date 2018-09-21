using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions.Abstractions;

namespace HBD.EntityFrameworkCore.Extensions.Internal
{
    internal static class Extensions
    {
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

        internal static void RegisterMappingFromExtension(this ModelBuilder modelBuilder, IEnumerable<RegistrationInfo> registrations)
        {
            foreach (var reg in registrations)
                foreach (var type in reg.GetMappers())
                    modelBuilder.RegisterMapping(type);
        }
    }
}
