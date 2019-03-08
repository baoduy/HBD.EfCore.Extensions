using System.Threading.Tasks;
using DataLayer;
using FluentAssertions;
using HBD.EfCore.Extensions.OrderBuilders;
using HBD.EfCore.Extensions.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class OrderBuilderTests
    {
        [TestMethod]
        public async Task TestOrderBuilder_User_ByProps()
        {
            await UnitTestSetup.Db.SeedData();

            var orderBuilder = OrderBuilder.CreateBuilder<User>()
                .OrderBy(u => u.CreatedBy)
                .ThenBy(u => u.FirstName);

            var list = await orderBuilder.Build(UnitTestSetup.Db.Set<User>()).ToListAsync();

            list.Should().NotBeEmpty();
        }

        [TestMethod]
        public async Task TestOrderBuilder_User_ByPropsString()
        {
            await UnitTestSetup.Db.SeedData();

            var orderBuilder = OrderBuilder.CreateBuilder<User>()
                .OrderBy(nameof(User.CreatedBy))
                .ThenBy(nameof(User.FirstName));

            var list = await orderBuilder.Build(UnitTestSetup.Db.Set<User>()).ToListAsync();

            list.Should().NotBeEmpty();
        }

        [TestMethod]
        public async Task TestOrderBuilder_User_ByProps_Desc()
        {
            await UnitTestSetup.Db.SeedData();

            var orderBuilder = OrderBuilder.CreateBuilder<User>()
                .OrderByDescending(u => u.CreatedBy)
                .ThenByDescending(u => u.FirstName);

            var list = await orderBuilder.Build(UnitTestSetup.Db.Set<User>()).ToListAsync();

            list.Should().NotBeEmpty();
        }

        [TestMethod]
        public async Task TestOrderBuilder_User_ByPropsString_Desc()
        {
            await UnitTestSetup.Db.SeedData();

            var orderBuilder = OrderBuilder.CreateBuilder<User>()
                .OrderByDescending(nameof(User.CreatedBy))
                .ThenByDescending(nameof(User.FirstName));

            var list = await orderBuilder.Build(UnitTestSetup.Db.Set<User>()).ToListAsync();

            list.Should().NotBeEmpty();
        }

        [TestMethod]
        public async Task TestOrderBuilder_User_OrderWith()
        {
            await UnitTestSetup.Db.SeedData();

            var orderBuilder = OrderBuilder.CreateBuilder<User>()
                .OrderByDescending(u => u.UpdatedOn)
                .ThenByDescending(nameof(User.FirstName));

            var list = await UnitTestSetup.Db.Set<User>().OrderWith(orderBuilder).ToListAsync();

            list.Should().NotBeEmpty();
        }
    }
}
