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
    public static class DataSeedingConfigurationExtensions
    {
        #region Fields

        private static readonly MethodInfo Method = typeof(DataSeedingConfigurationExtensions)
                    .GetMethod(nameof(RegisterData), BindingFlags.Static | BindingFlags.NonPublic);

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Register EntityTypeConfiguration from RegistrationInfos <see cref="RegistrationInfo" />
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="registrations"></param>
        internal static void RegisterDataSeedingFrom(this ModelBuilder modelBuilder,
            IEnumerable<RegistrationInfo> registrations)
        {
            foreach (var type in registrations.SelectMany(GetDataSeedingTypes))
                modelBuilder.HasData(type);
        }

        private static Type[] GetDataSeedingTypes(this ICollection<Assembly> assemblies)
        {
            return assemblies.ToArray().Extract().Class().NotAbstract().NotGeneric().NotInterface()
                .IsInstanceOf(typeof(IDataSeedingConfiguration<>)).Distinct().ToArray();
        }

        private static Type[] GetDataSeedingTypes(this RegistrationInfo @this)
            => GetDataSeedingTypes(@this.EntityAssemblies);

        private static void HasData(this ModelBuilder modelBuilder, Type mapperType)
        {
            if (mapperType == null) throw new ArgumentNullException(nameof(mapperType));

            var eType = HBD.EfCore.Extensions.EfCoreExtensions.GetEntityType(mapperType);

            if (Method == null || eType == null)
                throw new ArgumentException($"The {nameof(RegisterData)} or EntityType are not found");

            var md = Method.MakeGenericMethod(eType, mapperType);
            md.Invoke(null, new object[] { modelBuilder });
        }

        private static ModelBuilder RegisterData<TEntity, TMapping>(this ModelBuilder builder)
            where TMapping : IDataSeedingConfiguration<TEntity>
            where TEntity : class
        {
            var dataSeeding = (IDataSeedingConfiguration<TEntity>)Activator.CreateInstance(typeof(TMapping));

            //dataSeeding.Apply(builder.Entity<TEntity>());
            builder.Entity<TEntity>().HasData(dataSeeding.Data.ToArray());
            return builder;
        }

        #endregion Methods
    }
}