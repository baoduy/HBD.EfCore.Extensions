using System.Runtime.CompilerServices;
using HBD.EntityFrameworkCore.Extensions.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: InternalsVisibleTo("HBD.EntityFrameworkCore.Extensions.Tests")]
namespace DataLayer.Mappers
{
    internal class BaseEntityMapper<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        public static bool Called { get; private set; } = false;

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            Called = true;
            base.Configure(builder);
            builder.HasIndex(c => c.Id).IsUnique();
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
