using HBD.EfCore.Extensions;
using Domains.Abstracts;
using Domains.Owned;
using Domains.Profiles;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Accounts
{
    [Table("Accounts", Schema = DomainSchemas.Account)]
    public abstract class Account : EntityBase
    {
        #region Constructors

        protected Account(Guid profileId, Address address, string userName)
        {
            ProfileId = profileId;
            UpdateAddress(address, userName);
        }

        protected Account()
        {
        }

        #endregion Constructors

        #region Properties

        public Address Address { get; private set; }

        public virtual Profile Profile { get; private set; }

        public Guid ProfileId { get; private set; }

        #endregion Properties

        #region Methods

        public void UpdateAddress(Address address, string userName)
        {
            Address.UpdateFrom(address);
            SetUpdatedBy(userName);
        }

        #endregion Methods
    }
}