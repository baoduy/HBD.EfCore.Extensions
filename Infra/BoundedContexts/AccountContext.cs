using Domains.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Repos.BoundedContexts
{
    internal class AccountContext : DbContextBase
    {
        #region Constructors

        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }

        #endregion Constructors

        #region Properties

        public virtual DbSet<BusinessAccount> BusinessAccounts { get; private set; }

        public virtual DbSet<PersonalAccount> PersonalAccounts { get; private set; }

        #endregion Properties
    }
}