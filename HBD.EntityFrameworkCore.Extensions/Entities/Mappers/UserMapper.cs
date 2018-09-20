using HBD.EntityFrameworkCore.Extensions.Mappers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mappers
{
    internal class UserMapper : EntityMapper<User>
    {
        public override void Map(EntityTypeBuilder<User> builder)
        {
            base.Map(builder);
            builder.HasIndex(u => u.FirstName);
        }
    }
}
