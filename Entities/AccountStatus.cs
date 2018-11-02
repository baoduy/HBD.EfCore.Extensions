using System.ComponentModel.DataAnnotations;
using HBD.EntityFrameworkCore.Extensions.Abstractions;

namespace DataLayer
{
    public class AccountStatus : Entity
    {
        public AccountStatus() { }
        public AccountStatus(long id):base(id) { }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
