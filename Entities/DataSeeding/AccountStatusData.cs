using HBD.EntityFrameworkCore.Extensions.Configurations;

namespace DataLayer.DataSeeding
{
    public class AccountStatusData : IDataSeedingConfiguration<AccountStatus>
    {
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
    }
}
