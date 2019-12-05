using HBD.EfCore.DDD.Attributes;
using HBD.EfCore.DDD.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HBD.EfCore.DDD.Tests.Infra
{
    public class Profile : AggregateRoot
    {
        #region Fields

        private readonly HashSet<Account> accounts = new HashSet<Account>();

        #endregion Fields

        #region Constructors

        public Profile(string name, string userId) : base(userId) => Name = name;

        private Profile()
        {
        }

        #endregion Constructors

        #region Properties

        public IReadOnlyCollection<Account> Accounts => accounts;

        public string Name { get; private set; }

        #endregion Properties

        #region Methods

        public void AddAccount(string accountName, string userId)
        {
            var acc = new Account(this.Id, accountName, userId);
            AddAccount(acc);
        }

        internal void AddAccount(Account account)
        {
            this.accounts.Add(account);
            AddEvent(new AccountEvent { Id = account.Id, Name = account.Name }, GenericEventRunner.ForEntities.EventToSend.BeforeAndAfterSave);

            SetUpdatedBy(account.LastModifiedBy);
        }

        internal void AddAccounts(IEnumerable<Account> accounts)
        {
            foreach (var acc in accounts)
                AddAccount(acc);
        }

        [Event(typeof(AccountEvent))]
        public Account RemoveAccount(Guid accountId, string userId)
        {
            var ac = accounts.First(a => a.Id == accountId);
            this.accounts.Remove(ac);
            SetUpdatedBy(userId);

            //EventActionRunner will use this return to map to event.
            return ac;
        }

        #endregion Methods
    }
}