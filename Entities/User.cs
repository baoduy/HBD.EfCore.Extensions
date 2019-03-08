using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer
{
    public sealed class User : BaseEntity
    {
        #region Public Constructors

        public User(string userName) : this(0, userName)
        {
        }

        public User(int id, string userName) : base(id, userName)
        {
        }

        public User()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public Account Account { get; set; }

        /// <summary>
        /// Private Set for Data Seeding purpose.
        /// </summary>
        public ICollection<Address> Addresses { get; private set; } = new HashSet<Address>();

        [Required] [MaxLength(256)] public string FirstName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required] [MaxLength(256)] public string LastName { get; set; }
        public HashSet<Payment> Payments { get; private set; } = new HashSet<Payment>();

        [Column(TypeName = "Money")]
        public decimal TotalPayment { get; private set; }

        [NotMapped]
        public bool TotalPaymentCalculated { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void UpdatedByUser(string userName) => SetUpdatedBy(userName);

        #endregion Public Methods
    }
}