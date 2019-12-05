using HBD.EfCore.DDD.Tests.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using HBD.EfCore.DDD.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using System.Linq;
namespace HBD.EfCore.DDD.Tests
{
    [TestClass]
    public class ReadOnlyDtoRepositoryTests
    {
        [TestMethod]
        public async System.Threading.Tasks.Task ReadAsync()
        {
            var db = Initialize.Provider.GetRequiredService<ProfileContext>();
            var repo = Initialize.Provider.GetRequiredService<IDtoReadOnlyRepository<Profile>>();

            var rs = await repo.ReadAsync<ProfileViewModel>().ToListAsync();
            rs.Should().HaveCountGreaterOrEqualTo(1);
            rs.All(i => i.Name != null).Should().BeTrue();
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadSingleAsync()
        {
            var db = Initialize.Provider.GetRequiredService<ProfileContext>();
            var repo = Initialize.Provider.GetRequiredService<IDtoReadOnlyRepository<Profile>>();

            var p = db.Set<Profile>().First();
            var rs = await repo.ReadAsync<ProfileViewModel>(i => i.Id == p.Id).ToListAsync();

            rs.Should().HaveCount(1);
            rs.First().Id.Should().Be(p.Id);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadPageAsync()
        {
            var db = Initialize.Provider.GetRequiredService<ProfileContext>();
            var repo = Initialize.Provider.GetRequiredService<IDtoReadOnlyRepository<Profile>>();

            var rs = await repo.ReadPageAsync<ProfileViewModel>(1, 10, OrderBuilder.CreateBuilder<ProfileViewModel>(i => i.Id));

            rs.Items.Should().HaveCount(10);
        }
    }
}