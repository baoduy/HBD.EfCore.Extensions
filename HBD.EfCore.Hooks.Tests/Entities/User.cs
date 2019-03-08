using HBD.EfCore.Hooks.SavingAwareness;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.EfCore.Hooks.Tests.Entities
{
    public class User : ISavingAware, IValidatableObject
    {
        #region Public Properties

        [Required] [MaxLength(256)] public string FirstName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [Key]
        public int Id { get; set; }

        [Required] [MaxLength(256)] public string LastName { get; set; }

        public HashSet<Payment> Payments { get; private set; } = new HashSet<Payment>();

        [NotMapped]
        public int SavingCalled { get; private set; }

        [Column(TypeName = "Money")] public decimal TotalPayment { get; private set; }
        [NotMapped] public bool TotalPaymentCalculated { get; private set; }

        [NotMapped]
        public bool Validated { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public Task OnSavingAsync(EntityState state, DbContext dbContext)
        {
            SavingCalled += 1;

            if (state == EntityState.Deleted) return Task.CompletedTask;

            if (dbContext.Entry(this).HasChangeOn(i => i.Payments))
            {
                TotalPaymentCalculated = true;
                TotalPayment = Payments.Any() ? Payments.Sum(i => i.Amount) : 0;
            }

            return Task.CompletedTask;
        }

        public void Reset()
        {
            TotalPaymentCalculated = false;
            SavingCalled = 0;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SavingCalled <= 0)
                throw new InvalidOperationException($"{nameof(OnSavingAsync)} should run before {nameof(Validate)}");

            Validated = true;
            yield break;
        }

        #endregion Public Methods
    }
}