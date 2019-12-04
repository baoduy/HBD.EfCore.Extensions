using FluentAssertions;
using HBD.EfCore.EntityResolver.Tests.Entities;
using HBD.EfCore.EntityResolver.Tests.Helpers;
using HBD.EfCore.EntityResolver.Tests.Models;
using HBD.EfCore.EntityResolvers;
using HBD.TestHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace HBD.EfCore.EntityResolver.Tests
{
    [TestClass]
    public class TestEntityResolverAsync
    {
        #region Methods

        [TestMethod]
        public async Task Test_AlwaysIncluded()
        {
            var db = ResolverTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = ResolverTestSetup.Provider.GetService<IEntityResolverAsync>();

            var rs = await resolver.ResolveAsync(new UserModel()).ConfigureAwait(false);
            (rs.CreateDateTime as DateTime?).Should().BeOnOrAfter(DateTime.Today);

            rs = await resolver.ResolveAsync(new UserModel(), true).ConfigureAwait(false);
            (rs.CreateDateTime as DateTime?).Should().BeOnOrAfter(DateTime.Today);
        }

        [TestMethod]
        public async Task Test_Collection_Items()
        {
            var db = ResolverTestSetup.Db;
            await db.SeedData().ConfigureAwait(false);

            var resolver = new EntityResolverAsync(db, null);

            var result = await resolver.ResolveAsync(new UserModel
            {
                ListAccounts = new[] {
                    new AccountBasicViewModel { Id = 1 },
                    new AccountBasicViewModel { Id = 2 } }
            }).ConfigureAwait(false);

            (result.ListAccounts.Count as int?).Should().Be(2);
        }

        [TestMethod]
        public async Task Test_Generic_ResolveAndMap_Exception()
        {
            var db = ResolverTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = ResolverTestSetup.Provider.GetService<IEntityResolverAsync>();

            var result = await resolver.ResolveAndMapAsync<User>(new object()).ConfigureAwait(false);
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task Test_Resolve_Exception()
        {
            var db = ResolverTestSetup.Db;
            await db.SeedData().ConfigureAwait(false);

            var resolver = ResolverTestSetup.Provider.GetService<IEntityResolverAsync>();

            var result = await resolver.ResolveAsync(new object()).ConfigureAwait(false);
            ((object)result).Should().BeNull();
        }

        [TestMethod]
        public async Task Test_ResolveAndMap()
        {
            var db = ResolverTestSetup.Db;
            await db.SeedData().ConfigureAwait(false);

            var resolver = ResolverTestSetup.Provider.GetService<IEntityResolverAsync>();

            var user = await resolver.ResolveAndMapAsync<User>(new UserModel
            {
                ListAccounts = new[] {
                    new AccountBasicViewModel { Id = 1 },
                    new AccountBasicViewModel { Id = 2 }
                }
            }).ConfigureAwait(false);

            user.Should().NotBeNull();
            user.ListAccounts.Should().HaveCount(2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Test_ResolveAndMap_Error()
        {
            var db = ResolverTestSetup.Db;
            await db.SeedData().ConfigureAwait(false);

            var resolver = new EntityResolverAsync(db, null);
            var user = await resolver.ResolveAndMapAsync<User>(new UserModel
            {
                ListAccounts = new[] {
                    new AccountBasicViewModel { Id = 1 },
                    new AccountBasicViewModel { Id = 2 } }
            }).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task Test_ResolveAndMap_Exception()
        {
            var db = ResolverTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = ResolverTestSetup.Provider.GetService<IEntityResolverAsync>();

            await resolver.ResolveAndMapAsync(new object(), new User()).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task Test_SingleItem()
        {
            var db = ResolverTestSetup.Db;
            await db.SeedData().ConfigureAwait(false);

            var resolver = new EntityResolverAsync(db, null);

            var result = await resolver.ResolveAsync(new UserModel
            {
                AccountId = 1,
                OtherAccount = new AccountBasicViewModel { Id = 2 }
            }).ConfigureAwait(false);

            (result.Account as Account).Should().NotBeNull();
            (result.OtherAccount as Account).Should().NotBeNull();
        }

        [TestMethod]
        public async Task Test_Without_Spec_Item()
        {
            var db = ResolverTestSetup.Db;
            await db.SeedData().ConfigureAwait(false);

            var resolver = new EntityResolverAsync(db, null);

            var result = await resolver.ResolveAsync(new UserModel
            {
                OtherAccountWithoutSpec = new AccountBasicViewModel { Id = 1 }
            }).ConfigureAwait(false);

            (result.OtherAccountWithoutSpec as Account).Should().NotBeNull();
        }

        #endregion Methods
    }
}