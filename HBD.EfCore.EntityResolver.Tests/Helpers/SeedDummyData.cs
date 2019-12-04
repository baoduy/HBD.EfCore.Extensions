using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using HBD.EfCore.EntityResolver.Tests.Entities;

namespace HBD.EfCore.EntityResolver.Tests.Helpers
{
    public static class SeedDummyData
    {
        #region Public Methods

        public static async Task SeedData(this MyDbContext @this, int number = 100, bool force = false)
        {
            if (!force && @this.Set<User>().Count() >= number) return;

            await @this.Set<User>().AddRangeAsync(GenerateUsers(number)).ConfigureAwait(false);
            await @this.SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion Public Methods

        #region Private Methods

        private static List<Account> GenerateAccount(int number)
        {
            return new Faker<Account>()
                 .RuleFor(u => u.UserName, f => f.Person.UserName)
                 .RuleFor(u => u.Password, f => f.Person.Random.String(6, 15))
                 .RuleFor(u => u.CreatedBy, f => f.Name.FirstName())
                 .RuleFor(u => u.UpdatedBy, f => f.Name.FirstName())
                 .RuleFor(u => u.CreatedOn, f => f.Date.SoonOffset())
                 .RuleFor(u => u.UpdatedOn, f => f.Date.SoonOffset())
                 .Generate(number);
        }

        private static IEnumerable<User> GenerateUsers(int number)
        {
            return new Faker<User>()
                 .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                 .RuleFor(u => u.ListAccounts, f => GenerateAccount(2))
                 .RuleFor(u => u.LastName, f => f.Name.LastName())
                 .Generate(number);
        }

        #endregion Private Methods
    }
}