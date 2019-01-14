using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HBD.EfCore.Extensions.Attributes;
using HBD.EfCore.Extensions.Internal;
using HBD.EfCore.Extensions.Options;
using HBD.Framework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Extensions
{
    public static class StaticDataEnumEntityExtensions
    {
        #region Public Methods

        /// <summary>
        /// Register StaticDataOfAttribute Entities from RegistrationInfos <see cref="RegistrationInfo"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="registrations"></param>
        public static void RegisterStaticDataFrom(this ModelBuilder modelBuilder,
            IEnumerable<RegistrationInfo> registrations)
        {
            foreach (var type in registrations.SelectMany(r =>
                r.EntityAssemblies.Extract().Enum().HasAttribute<StaticDataAttribute>()))
                modelBuilder.RegisterStaticDataFromEnumType(type);
        }

        #endregion Public Methods

        #region Private Methods

        private static void RegisterStaticDataFromEnumType(this ModelBuilder modelBuilder, Type enumType)
        {
            var enumTableType = typeof(EnumTables<>).MakeGenericType(enumType);
            var method = typeof(StaticDataEnumEntityExtensions).GetMethod(nameof(RegisterStaticDataType),
                BindingFlags.Static | BindingFlags.NonPublic);

            if (method == null)
                throw new ArgumentException($"The {nameof(RegisterStaticDataType)} is not found");

            var md = method.MakeGenericMethod(enumTableType);
            md.Invoke(null, new object[] { modelBuilder });
        }

        private static void RegisterStaticDataType<TEnumTable>(ModelBuilder modelBuilder) where TEnumTable : class
        {
            var enumType = typeof(TEnumTable).GetGenericArguments().First();
            var builder = modelBuilder.Entity<TEnumTable>().ToTable(enumType.Name);

            #region Add EnumStatus as Data Seeding

            builder.Property<int>("Id").IsRequired().ValueGeneratedNever();
            builder.HasKey("Id");

            builder.Property<string>("Name").HasMaxLength(100)
                .IsRequired();

            foreach (var value in Enum.GetValues(enumType))
                builder.HasData(new { Id = (int)value, Name = value.ToString() });

            #endregion Add EnumStatus as Data Seeding
        }

        #endregion Private Methods
    }
}