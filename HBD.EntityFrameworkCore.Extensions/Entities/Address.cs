using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HBD.EntityFrameworkCore.Extensions.Abstractions;

namespace DataLayer
{
    public class Address: Entity<int>
    {
        [Required]
        [MaxLength(256)]
        public string Street { get; set; }

        [ForeignKey("Address_User")]
        public long UserId { get; set; }

        public virtual User User { get; set; }
    }
}
