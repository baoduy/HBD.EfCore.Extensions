using HBD.EfCore.DDD.Domains;
using System;

namespace HBD.EfCore.DDD.Tests.Infra
{
    public class Account : DomainEntity
    {
        #region Constructors

        public Account(Guid profileId, string name, string userId) : base(userId)
        {
            Name = name;
            ProfileId = profileId;
        }

        private Account()
        {
        }

        #endregion Constructors

        #region Properties

        public string Name { get; private set; }

        public virtual Profile Profile { get; private set; }

        public Guid ProfileId { get; private set; }

        #endregion Properties
    }
}