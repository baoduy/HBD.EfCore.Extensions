using System;
using System.Linq;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HBD.EntityFrameworkCore.Extensions.Configurations
{
    public class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            #region Add EnumStatus as Data Seeding

            var att = typeof(TEntity).GetCustomAttribute<StaticDataOfAttribute>();
            if (att == null) return;

            builder.Property<int>("Id").IsRequired();
            builder.HasKey("Id");

            builder.Property<string>("Name").HasMaxLength(100)
                .IsRequired();
            builder.Property<int>("Value")
                .IsRequired();

            var index = 1;
            foreach (var value in Enum.GetValues(att.EnumType))
                builder.HasData(new { Id = index++, Name = value.ToString(), Value = (int)value });
            #endregion
        }
    }
}
