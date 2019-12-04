using HBD.Actions.Runner.Attributes;
using System;

namespace HBD.EfCore.DDD.Tests.Infra
{
    [ActionMap(nameof(Profile.RemoveAccount))]
    public class AccountDto
    {
        #region Properties

        public Guid AccountId { get; set; }

        public string UserId { get; set; }

        #endregion Properties
    }
}