using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions.Attributes;
using HBD.EntityFrameworkCore.Extensions.Configurations;
using HBD.EntityFrameworkCore.Extensions.Options;

namespace HBD.EntityFrameworkCore.Extensions
{
    public static class StaticDataEnumEntityExtensions
    {
        internal static void RegisterStaticDataFromType(this ModelBuilder modelBuilder, Type entityType)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));
            var method = typeof(EntityTypeConfigurationExtensions).GetMethod(nameof(EntityTypeConfigurationExtensions.RegisterMapping), BindingFlags.Static | BindingFlags.NonPublic);

            if (method == null)
                throw new ArgumentException($"The {nameof(EntityTypeConfigurationExtensions.RegisterMapping)}");

            var mapperType = typeof(EntityTypeConfiguration<>).MakeGenericType(entityType);
            var md = method.MakeGenericMethod(entityType, mapperType);
            md.Invoke(null, new object[] { modelBuilder });
        }

        /// <summary>
        /// Register StaticDataOfAttribute Entities from RegistrationInfos <see cref="RegistrationInfo"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="registrations"></param>
        public static void RegisterStaticDataFrom(this ModelBuilder modelBuilder, IEnumerable<RegistrationInfo> registrations)
        {
            foreach (var type in registrations.SelectMany(r => r.EntityAssemblies.Extract().Class().HasAttribute<StaticDataOfAttribute>()))
                modelBuilder.RegisterStaticDataFromType(type);
        }
    }
}
