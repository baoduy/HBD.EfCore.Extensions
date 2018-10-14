using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mappers
{
    internal class UserEntityMapper: CustomEntityMapper<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.HasOne(c => c.Account).WithOne(c => c.User)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
