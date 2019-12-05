using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using HBD.EfCore.DDD.Repositories;
using HBD.EfCore.DDD.Tests.Infra;
using Microsoft.Extensions.DependencyInjection;

namespace HBD.EfCore.DDD.Tests
{
    [TestClass]
    public class ReadOnlyRepositoryTests
    {
        #region Methods

        [TestMethod]
        public async System.Threading.Tasks.Task ReadAsync()
        {
            var db = Initialize.Provider.GetRequiredService<ProfileContext>();
            var repo = Initialize.Provider.GetRequiredService<IReadOnlyRepository<Profile>>();

            var users = await repo.ReadAsync().ToListAsync();
            users.Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadConditionAsync()
        {
            var db = Initialize.Provider.GetRequiredService<ProfileContext>();
          var repo = Initialize.Provider.GetRequiredService<IReadOnlyRepository<Profile>>();

            var users = await repo.ReadAsync(u => u.Name.Contains("D")).ToListAsync();
            users.Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadPageAsync()
        {
            var db = Initialize.Provider.GetRequiredService<ProfileContext>();
          var repo = Initialize.Provider.GetRequiredService<IReadOnlyRepository<Profile>>();

            var users = await repo.ReadPageAsync(0, 10, u => u.Name);
            users.Items.Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadPageWithoutFiltersAsync()
        {
            var db = Initialize.Provider.GetRequiredService<ProfileContext>();
            var repo = Initialize.Provider.GetRequiredService<IReadOnlyRepository<Profile>>();

            var users = await repo.ReadPageIgnoreFiltersAsync(0, 10, u => u.Name);
            users.Items.Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadSingleAsync()
        {
            var db = Initialize.Provider.GetRequiredService<ProfileContext>();
            var repo = Initialize.Provider.GetRequiredService<IReadOnlyRepository<Profile>>();

            var p = await db.Set<Profile>().FirstAsync();
            var u = await repo.ReadSingleAsync(p.Id);
            u.Should().NotBeNull();
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadWithoutFiltersAsync()
        {
            var db = Initialize.Provider.GetRequiredService<ProfileContext>();
            var repo = Initialize.Provider.GetRequiredService<IReadOnlyRepository<Profile>>();

            var users = await repo.ReadIgnoreFiltersAsync().ToListAsync();
            users.Should().HaveCountGreaterOrEqualTo(1);
        }

        #endregion Methods
    }
}