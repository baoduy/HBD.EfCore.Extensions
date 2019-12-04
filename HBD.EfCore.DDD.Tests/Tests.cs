using FluentAssertions;
using HBD.Actions.Runner;
using HBD.EfCore.DDD.Internals;
using HBD.EfCore.DDD.Tests.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HBD.EfCore.DDD.Tests
{
    [TestClass]
    public class Tests
    {
        #region Methods

        [TestMethod]
        public void ActionRunnerSetup()
        {
            var runner = Initialize.Provider.GetRequiredService<IActionRunnerService>();
            runner.Should().BeOfType<EventActionRunner>();
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AddAccountAsync()
        {
            var db = Initialize.Provider.GetRequiredService<ProfileContext>();

            AccountAfterSaveEventHandler.Reset();

            var p = new Profile("P1", "Duy");
            p.AddAccount("A1", "Duy");

            db.Add(p);
            await db.SaveChangesAsync();

            AccountAfterSaveEventHandler.Called.Should().BeTrue();
        }

        [TestMethod]
        public async System.Threading.Tasks.Task CreateProfileAsync()
        {
            var db = Initialize.Provider.GetService<ProfileContext>();

            var p = new Profile("P1", "Duy");
            db.Add(p);
            await db.SaveChangesAsync();

            var r = new Repositories.ReadOnlyRepository<Profile>(db);
            var p1 = await r.ReadSingleAsync(p.Id);

            p1.Should().NotBeNull();
        }

        [TestMethod]
        public async System.Threading.Tasks.Task RemoveAccountAsync()
        {
            await AddAccountAsync();
            var runner = Initialize.Provider.GetRequiredService<IActionRunnerService>();

            AccountAfterSaveEventHandler.Reset();
            var db = Initialize.Provider.GetService<ProfileContext>();

            var p = await db.Set<Profile>().Include(i => i.Accounts).FirstAsync();

            runner.Run(p, new AccountDto
            {
                AccountId = p.Accounts.First().Id,
                UserId = "Duy"
            });

            await db.SaveChangesAsync();

            AccountAfterSaveEventHandler.Called.Should().BeTrue();
        }

        #endregion Methods
    }
}