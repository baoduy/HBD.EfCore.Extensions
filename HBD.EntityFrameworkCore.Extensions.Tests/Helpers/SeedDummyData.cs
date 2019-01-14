using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using DataLayer;

namespace HBD.EntityFrameworkCore.Extensions.Tests.Helpers
{
    public static class SeedDummyData
    {
        #region Public Methods

        public static async Task SeedData(this MyDbContext @this, int number = 100, bool force = false)
        {
            if (!force && @this.Set<User>().Count() >= number) return;

            await @this.Set<User>().AddRangeAsync(GenerateUsers(number));
            await @this.SaveChangesAsync();
        }

        #endregion Public Methods

        #region Private Methods

        private static Account GenerateAccount()
        {
            return new Faker<Account>()
                 .RuleFor(u => u.UserName, f => f.Person.UserName)
                 .RuleFor(u => u.Password, f => f.Person.Random.String(6, 15))
                 .RuleFor(u => u.CreatedBy, f => f.Name.FirstName())
                 .RuleFor(u => u.UpdatedBy, f => f.Name.FirstName())
                 .RuleFor(u => u.CreatedOn, f => f.Date.SoonOffset())
                 .RuleFor(u => u.UpdatedOn, f => f.Date.SoonOffset())
                 .Generate(1).First();
        }

        private static IEnumerable<Address> GenerateAddresses()
        {
            return new Faker<Address>()
                .RuleFor(u => u.Street, f => f.Address.StreetName())
                .Generate(3);
        }

        private static IEnumerable<User> GenerateUsers(int number)
        {
            return new Faker<User>()
                 .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                 .RuleFor(u => u.Account, f => GenerateAccount())
                 .RuleFor(u => u.Addresses, f => GenerateAddresses())
                 .RuleFor(u => u.LastName, f => f.Name.LastName())
                 .RuleFor(u => u.CreatedBy, f => f.Name.FirstName())
                 .RuleFor(u => u.UpdatedBy, f => f.Name.FirstName())
                 .RuleFor(u => u.CreatedOn, f => f.Date.SoonOffset())
                 .RuleFor(u => u.UpdatedOn, f => f.Date.SoonOffset())
                 .Generate(number);
        }

        #endregion Private Methods
    }
}