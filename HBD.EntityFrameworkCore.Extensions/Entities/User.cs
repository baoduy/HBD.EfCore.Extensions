using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using HBD.EntityFrameworkCore.Extensions.Mappers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer
{
    public class User : AuditEntity
    {
        [Required]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(256)]
        public string LastName { get; set; }

        //[ForeignKey("User_Account")]
        public long AccountId { get; set; }

        public virtual Account Account { get; set; }

        public virtual ICollection<Address> Addresses { get; } = new HashSet<Address>();
    }

    internal class UserMapper : EntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.HasIndex(u => u.FirstName);
        }
    }
}
