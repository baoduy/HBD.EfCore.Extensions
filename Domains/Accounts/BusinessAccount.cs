using Domains.Abstracts;
using Domains.Owned;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Domains.Accounts
{
    public class BusinessAccount : Account
    {
        #region Constructors

        public BusinessAccount(Guid profileId, Address address, string userName) : base(profileId, address, userName)
        {
        }

        private BusinessAccount()
        {
        }

        #endregion Constructors

        #region Methods

        public override ValueTask<bool> IsDeletableAsync(DeleteType type, DbContext service) => new ValueTask<bool>(true);

        #endregion Methods
    }
}