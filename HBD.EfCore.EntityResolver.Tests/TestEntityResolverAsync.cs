using FluentAssertions;
using HBD.EfCore.EntityResolver.Tests.Entities;
using HBD.EfCore.EntityResolver.Tests.Helpers;
using HBD.EfCore.EntityResolver.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace HBD.EfCore.EntityResolver.Tests
{
    [TestClass]
    public class TestEntityResolverAsync
    {
        [TestMethod]
        public async Task Test_SingleItem()
        {
            var db = UnitTestSetup.Db;
            await db.SeedData();

            var resolver = new EntityResolverAsync(db, null);

            var result = await resolver.ResolveAsync(new UserModel
            {
                AccountId = 1,
                OtherAccount = new AccountBasicViewModel { Id = 2 }
            });

            (result.Account as Account).Should().NotBeNull();
            (result.OtherAccount as Account).Should().NotBeNull();
        }

        [TestMethod]
        public async Task Test_Without_Spec_Item()
        {
            var db = UnitTestSetup.Db;
            await db.SeedData();

            var resolver = new EntityResolverAsync(db, null);

            var result = await resolver.ResolveAsync(new UserModel
            {
                OtherAccountWithoutSpec = new AccountBasicViewModel { Id = 1 }
            });

            (result.OtherAccountWithoutSpec as Account).Should().NotBeNull();
        }

        [TestMethod]
        public async Task Test_Collection_Items()
        {
            var db = UnitTestSetup.Db;
            await db.SeedData();

            var resolver = new EntityResolverAsync(db, null);

            var result = await resolver.ResolveAsync(new UserModel
            {
                ListAccounts = new[] {
                    new AccountBasicViewModel { Id = 1 },
                    new AccountBasicViewModel { Id = 2 } }
            });

            (result.ListAccounts.Count as int?).Should().Be(2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Test_ResolveAndMap_Error()
        {
            var db = UnitTestSetup.Db;
            await db.SeedData();

            var resolver = new EntityResolverAsync(db, null);
            var user = await resolver.ResolveAndMapAsync<User>(new UserModel
            {
                ListAccounts = new[] {
                    new AccountBasicViewModel { Id = 1 },
                    new AccountBasicViewModel { Id = 2 } }
            });
        }

        [TestMethod]
        public async Task Test_ResolveAndMap()
        {
            var db = UnitTestSetup.Db;
            await db.SeedData();

            var resolver = UnitTestSetup.Provider.GetService<IEntityResolverAsync>();

            var user = await resolver.ResolveAndMapAsync<User>(new UserModel
            {
                ListAccounts = new[] {
                    new AccountBasicViewModel { Id = 1 },
                    new AccountBasicViewModel { Id = 2 }
                }
            });

            user.Should().NotBeNull();
            user.ListAccounts.Should().HaveCount(2);
        }

        [TestMethod]
        public async Task Test_Resolve_Exception()
        {
            var db = UnitTestSetup.Db;
            await db.SeedData();

            var resolver = UnitTestSetup.Provider.GetService<IEntityResolverAsync>();

            var result = await resolver.ResolveAsync(new object());
            ((object)result).Should().BeNull();
        }

        [TestMethod]
        public async Task Test_Generic_ResolveAndMap_Exception()
        {
            var db = UnitTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = UnitTestSetup.Provider.GetService<IEntityResolverAsync>();

            var result = await resolver.ResolveAndMapAsync<User>(new object());
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task Test_ResolveAndMap_Exception()
        {
            var db = UnitTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = UnitTestSetup.Provider.GetService<IEntityResolverAsync>();

            await resolver.ResolveAndMapAsync(new object(), new User());
        }

        [TestMethod]
        public async Task Test_AlwaysIncluded()
        {
            var db = UnitTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = UnitTestSetup.Provider.GetService<IEntityResolverAsync>();

            var rs = await resolver.ResolveAsync(new UserModel());
            (rs.CreateDateTime as DateTime?).Should().BeOnOrAfter(DateTime.Today);

            rs = await resolver.ResolveAsync(new UserModel(), true);
            (rs.CreateDateTime as DateTime?).Should().BeOnOrAfter(DateTime.Today);
        }
    }
}
