using HBD.EfCore.Extensions.Configurations;

namespace DataLayer.DataSeeding
{
    public class AccountStatusData : IDataSeedingConfiguration<AccountStatus>
    {
        #region Public Properties

        public AccountStatus[] Data => new[]
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

        #endregion Public Properties
    }
}