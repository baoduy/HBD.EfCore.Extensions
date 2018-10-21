using System.Runtime.CompilerServices;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using HBD.EntityFrameworkCore.Extensions.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly:InternalsVisibleTo("HBD.EntityFrameworkCore.Extensions.Tests")]
namespace DataLayer.Mappers
{
    internal class CustomEntityMapper<T> : EntityTypeConfiguration<T> where T : class,IEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.HasIndex(c => c.Id).IsUnique();
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
