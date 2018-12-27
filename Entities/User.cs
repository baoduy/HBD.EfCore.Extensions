using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class User : BaseEntity, ISavingAwareness
    {
        #region Public Constructors

        public User(string userName) : base(userName)
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

        public virtual Account Account { get; set; }

        /// <summary>
        /// Private Set for Data Seeding purpose.
        /// </summary>
        public virtual ICollection<Address> Addresses { get; private set; } = new HashSet<Address>();

        [Required] [MaxLength(256)] public string FirstName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required] [MaxLength(256)] public string LastName { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void UpdatedByUser(string userName) => SetUpdatedBy(userName);

        #endregion Public Methods

        public int SavingCalled { get; private set; }

        public void OnSaving(EntityState state, DbContext dbContext)
        {
            SavingCalled += 1;
        }
    }
}