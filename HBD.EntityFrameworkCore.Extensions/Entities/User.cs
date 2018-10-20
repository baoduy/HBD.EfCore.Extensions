using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using HBD.EntityFrameworkCore.Extensions.Mappers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer
{
    public class User : AuditEntity
    {
        public User(string userName):base(userName) { }

        public User()
        {
        }

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

        public void UpdatedByUser(string userName) => this.SetUpdatedBy(userName);
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
