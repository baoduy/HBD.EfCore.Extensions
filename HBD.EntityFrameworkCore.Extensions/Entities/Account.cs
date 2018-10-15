using System.ComponentModel.DataAnnotations;
using HBD.EntityFrameworkCore.Extensions.Abstractions;

namespace DataLayer
{
    public class Account: AuditEntity
    {
        //public long UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
