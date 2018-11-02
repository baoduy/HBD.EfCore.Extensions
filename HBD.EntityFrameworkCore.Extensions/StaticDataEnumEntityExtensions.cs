using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using HBD.EntityFrameworkCore.Extensions.Attributes;
using HBD.EntityFrameworkCore.Extensions.Options;

namespace HBD.EntityFrameworkCore.Extensions
{
    public static class StaticDataEnumEntityExtensions
    {
        internal static void RegisterStaticDataType<TEnumTable>(ModelBuilder modelBuilder) where TEnumTable : class
        {
            var enumType = typeof(TEnumTable).GetGenericArguments().First();
            var builder = modelBuilder.Entity<TEnumTable>().ToTable(enumType.Name);

            #region Add EnumStatus as Data Seeding

            builder.Property<int>("Id").IsRequired();
            builder.HasKey("Id");

            builder.Property<int>("Key")
                .IsRequired();
            builder.Property<string>("Name").HasMaxLength(100)
                .IsRequired();

            var index = 1;
            foreach (var value in Enum.GetValues(enumType))
                builder.HasData(new { Id = index++, Name = value.ToString(), Key = (int)value });

            #endregion
        }

        internal static void RegisterStaticDataFromEnumType(this ModelBuilder modelBuilder, Type enumType)
        {
            var enumTableType = typeof(EnumTables<>).MakeGenericType(enumType);
            var method = typeof(StaticDataEnumEntityExtensions).GetMethod(nameof(RegisterStaticDataType), BindingFlags.Static | BindingFlags.NonPublic);

            if (method == null)
                throw new ArgumentException($"The {nameof(RegisterStaticDataType)} is not found");

            var md = method.MakeGenericMethod(enumTableType);
            md.Invoke(null, new object[] { modelBuilder });
        }

        /// <summary>
        /// Register StaticDataOfAttribute Entities from RegistrationInfos <see cref="RegistrationInfo"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="registrations"></param>
        public static void RegisterStaticDataFrom(this ModelBuilder modelBuilder, IEnumerable<RegistrationInfo> registrations)
        {
            foreach (var type in registrations.SelectMany(r => r.EntityAssemblies.Extract().Enum().HasAttribute<StaticDataAttribute>()))
                modelBuilder.RegisterStaticDataFromEnumType(type);
        }
    }
}
