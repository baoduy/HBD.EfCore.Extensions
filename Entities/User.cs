using HBD.EntityFrameworkCore.Extensions;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace DataLayer
{
    public class User : BaseEntity, ISavingAwareness
    {
        #region Public Constructors

        public User(string userName) : this(0, userName)
        {
        }

        public User(int id, string userName) : base(id, userName)
        {
            Payments = new HashSet<Payment>();
        }

        public User()
        {
            Payments = new HashSet<Payment>();
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual Account Account { get; set; }

        /// <summary>
        /// Private Set for Data Seeding purpose.
        /// </summary>
        public virtual ICollection<Address> Addresses { get; private set; } = new HashSet<Address>();

        [Required] [MaxLength(256)] public string FirstName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required] [MaxLength(256)] public string LastName { get; set; }
        public virtual HashSet<Payment> Payments { get; private set; }

        [NotMapped]
        public int SavingCalled { get; private set; }

        [Column(TypeName = "Money")]
        public decimal TotalPayment { get; private set; }

        [NotMapped]
        public bool TotalPaymentCalculated { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void OnSaving(EntityState state, DbContext dbContext)
        {
            SavingCalled += 1;

            if (state == EntityState.Deleted) return;

            if (dbContext.Entry(this).HasChangeOn(i => i.Payments))
            {
                TotalPaymentCalculated = true;
                TotalPayment = Payments.Any() ? Payments.Sum(i => i.Amount) : 0;
            }
        }

        public void Reset()
        {
            TotalPaymentCalculated = false;
            SavingCalled = 0;
        }

        public void UpdatedByUser(string userName) => SetUpdatedBy(userName);

        #endregion Public Methods
    }
}