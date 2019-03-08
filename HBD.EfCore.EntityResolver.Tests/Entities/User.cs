using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HBD.EfCore.Extensions.Abstractions;

namespace HBD.EfCore.EntityResolver.Tests.Entities
{
    public class User : Entity
    {
        #region Public Properties

        public virtual ICollection<Account> ListAccounts { get; set; } = new HashSet<Account>();

        [Required] [MaxLength(256)] public string FirstName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required] [MaxLength(256)] public string LastName { get; set; }

        [NotMapped] public int SavingCalled { get; private set; }

        [Column(TypeName = "Money")] public decimal TotalPayment { get; private set; }

        [NotMapped] public bool TotalPaymentCalculated { get; private set; }

        #endregion Public Properties
    }
}