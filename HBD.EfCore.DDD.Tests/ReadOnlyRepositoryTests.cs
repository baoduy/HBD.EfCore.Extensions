using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace HBD.EfCore.DDD.Tests
{
    [TestClass]
    public class ReadOnlyRepositoryTests
    {
        #region Methods

        [TestMethod]
        public async System.Threading.Tasks.Task ReadAsync()
        {
            await UnitTestSetup.Db.SeedData().ConfigureAwait(false);
            var repo = new ReadOnlyRepository<User>(UnitTestSetup.Db);

            var users = await repo.ReadAsync().ToList();
            users.Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadConditionAsync()
        {
            await UnitTestSetup.Db.SeedData().ConfigureAwait(false);
            var repo = new ReadOnlyRepository<User>(UnitTestSetup.Db);

            var users = await repo.ReadAsync(u => u.FirstName.StartsWith("D")).ToList();
            users.Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadPageAsync()
        {
            await UnitTestSetup.Db.SeedData().ConfigureAwait(false);
            var repo = new ReadOnlyRepository<User>(UnitTestSetup.Db);

            var users = await repo.ReadPageAsync(0, 10, u => u.FirstName);
            users.Items.Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadPageWithoutFiltersAsync()
        {
            await UnitTestSetup.Db.SeedData().ConfigureAwait(false);
            var repo = new ReadOnlyRepository<User>(UnitTestSetup.Db);

            var users = await repo.ReadPageIgnoreFildersAsync(0, 10, u => u.FirstName);
            users.Items.Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadSingleAsync()
        {
            await UnitTestSetup.Db.SeedData().ConfigureAwait(false);
            var repo = new ReadOnlyRepository<User>(UnitTestSetup.Db);

            var u = await repo.ReadSingleAsync(1);
            u.Should().NotBeNull();
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadSpecAsync()
        {
            await UnitTestSetup.Db.SeedData().ConfigureAwait(false);
            var repo = new ReadOnlyRepository<User>(UnitTestSetup.Db);

            var users = await repo.ReadSpecAsync(new UserIdGreaterThan10Spec(), OrderBuilder.CreateBuilder<User>(o => o.LastName)).ToList();
            users.Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadWithoutFiltersAsync()
        {
            await UnitTestSetup.Db.SeedData().ConfigureAwait(false);
            var repo = new ReadOnlyRepository<User>(UnitTestSetup.Db);

            var users = await repo.ReadIgnoreFildersAsync().ToList();
            users.Should().HaveCountGreaterOrEqualTo(1);
        }

        #endregion Methods
    }
}