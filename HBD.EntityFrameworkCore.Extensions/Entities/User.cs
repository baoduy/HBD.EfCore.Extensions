using HBD.EntityFrameworkCore.Extensions.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public virtual ICollection<Address> Addresses { get; } = new HashSet<Address>();
    }
}
