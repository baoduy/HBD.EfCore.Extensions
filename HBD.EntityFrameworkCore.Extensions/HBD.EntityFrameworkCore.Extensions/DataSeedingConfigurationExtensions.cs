using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions.Configurations;
using HBD.EntityFrameworkCore.Extensions.Options;

namespace HBD.EntityFrameworkCore.Extensions
{
    public static class DataSeedingConfigurationExtensions
    {
        internal static ModelBuilder RegisterData<TEntity, TMapping>(this ModelBuilder builder)
            where TMapping : IDataSeedingConfiguration<TEntity>
            where TEntity : class
        {
            var dataSeeding = (IDataSeedingConfiguration<TEntity>)Activator.CreateInstance(typeof(TMapping));
            //dataSeeding.Apply(builder.Entity<TEntity>());
            builder.Entity<TEntity>().HasData(dataSeeding.Data);
            return builder;
        }

        internal static void HasData(this ModelBuilder modelBuilder, Type mapperType)
        {
            if (mapperType == null) throw new ArgumentNullException(nameof(mapperType));
            var method = typeof(DataSeedingConfigurationExtensions).GetMethod(nameof(RegisterData), BindingFlags.Static | BindingFlags.NonPublic);
            var eType = Extensions.GetEntityType(mapperType);

            if (method == null || eType == null)
                throw new ArgumentException($"The {nameof(RegisterData)} or EntityType are not found");

            var md = method.MakeGenericMethod(eType, mapperType);
            md.Invoke(null, new object[] { modelBuilder });
        }

        internal static Type[] GetDataSeedingTypes(this Assembly[] assemblies)
            => assemblies.Extract().Class().NotAbstract().NotGeneric().NotInterface()
                .IsInstanceOf(typeof(IDataSeedingConfiguration<>)).ToArray();

        internal static Type[] GetDataSeedingTypes(this RegistrationInfo @this)
            => GetDataSeedingTypes(@this.EntityAssemblies);

        /// <summary>
        /// Register EntityTypeConfiguration from RegistrationInfos <see cref="RegistrationInfo"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="registrations"></param>
        public static void RegisterDataSeedingFromExtension(this ModelBuilder modelBuilder, IEnumerable<RegistrationInfo> registrations)
        {
            foreach (var type in registrations.SelectMany(GetDataSeedingTypes))
                modelBuilder.HasData(type);
        }
    }
}
