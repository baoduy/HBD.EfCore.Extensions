using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using HBD.EfCore.Extensions.Abstractions;
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
        internal static void RegisterStaticDataFrom(this ModelBuilder modelBuilder,
            IEnumerable<RegistrationInfo> registrations)
        {
            foreach (var type in registrations.SelectMany(r =>
                r.EntityAssemblies.Extract().Enum().HasAttribute<StaticDataAttribute>()).Distinct())
                modelBuilder.RegisterStaticDataFromEnumType(type);
        }

        #endregion Public Methods

        #region Private Methods

        private static readonly MethodInfo Method = typeof(StaticDataEnumEntityExtensions)
            .GetMethod(nameof(RegisterStaticDataType), BindingFlags.Static | BindingFlags.NonPublic);

        private static void RegisterStaticDataFromEnumType(this ModelBuilder modelBuilder, Type enumType)
        {
            var att = enumType.GetCustomAttribute<StaticDataAttribute>();
            var enumTableType = typeof(EnumTables<>).MakeGenericType(enumType);

            if (Method == null)
                throw new ArgumentException($"The {nameof(RegisterStaticDataType)} is not found");

            var md = Method.MakeGenericMethod(enumTableType);
            md.Invoke(null, new object[] { modelBuilder, att });
        }

        private static void RegisterStaticDataType<TEnumTable>(ModelBuilder modelBuilder, StaticDataAttribute attribute) where TEnumTable : class
        {
            var enumType = typeof(TEnumTable).GetGenericArguments().First();
            var builder = modelBuilder.Entity<TEnumTable>()
                .ToTable(attribute?.Table ?? enumType.Name, attribute?.Schema);

            #region Add EnumStatus as Data Seeding

            var columnsCreated = false;
            var hasDisplayAttribute = false;

            foreach (var value in Enum.GetValues(enumType))
            {
                var datt = enumType.GetMember(value.ToString())[0].GetCustomAttribute<DisplayAttribute>();

                if (!columnsCreated)
                {
                    builder.Property<int>(nameof(IEntity.Id))
                        .IsRequired()
                        .ValueGeneratedNever();

                    builder.HasKey(nameof(IEntity.Id));

                    builder.Property<string>(nameof(DisplayAttribute.Name))
                        .HasMaxLength(100)
                        .IsRequired();

                    if (datt != null)
                    {
                        builder.Property<string>(nameof(DisplayAttribute.Description))
                            .HasMaxLength(255);

                        builder.Property<string>(nameof(DisplayAttribute.GroupName))
                            .HasMaxLength(255);
                       
                        hasDisplayAttribute = true;
                    }

                    columnsCreated = true;
                }

                if (hasDisplayAttribute)
                    builder.HasData(new
                    {
                        Id = (int)value,
                        Name = datt?.Name ?? value.ToString(),
                        datt?.Description,
                        datt?.GroupName
                    });
                else builder.HasData(new
                {
                    Id = (int)value,
                    Name = value.ToString()
                });
            }

            #endregion Add EnumStatus as Data Seeding
        }

        #endregion Private Methods
    }
}