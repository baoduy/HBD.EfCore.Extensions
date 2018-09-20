using HBD.EntityFrameworkCore.Extensions.Abstractions;
using HBD.EntityFrameworkCore.Extensions.Mappers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mappers
{
    public class CustomEntityMapper<T> : EntityTypeConfiguration<T> where T : Entity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.HasIndex(c => c.Id).IsUnique();
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
