using HBD.EntityFrameworkCore.Extensions.Mappers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mappers
{
    internal class UserMapper : EntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.HasIndex(u => u.FirstName);
        }
    }
}
