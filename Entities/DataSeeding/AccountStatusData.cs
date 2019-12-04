using HBD.EfCore.Extensions.Configurations;
using System.Collections.Generic;

namespace DataLayer.DataSeeding
{
    public class AccountStatusData : IDataSeedingConfiguration<AccountStatus>
    {
        #region Properties

        public ICollection<AccountStatus> Data => new[]
        {
            new AccountStatus(1)
            {
                Name = "Duy"
            },
            new AccountStatus(2)
            {
                Name = "Hoang"
            }
        };

        #endregion Properties
    }
}