using FluentAssertions;
using HBD.EfCore.EntityResolver.Tests.Entities;
using HBD.EfCore.EntityResolver.Tests.Helpers;
using HBD.EfCore.EntityResolver.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HBD.EfCore.EntityResolver.Tests
{
    [TestClass]
    public class TestEntityResolver
    {
        [TestMethod]
        public void Test_SingleItem()
        {
            var db = UnitTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = new EntityResolver(db, null);

            var result = resolver.Resolve(new UserModel
            {
                AccountId = 1,
                OtherAccount = new AccountBasicViewModel { Id = 2 }
            });

            (result.Account as Account).Should().NotBeNull();
            (result.OtherAccount as Account).Should().NotBeNull();
        }

        [TestMethod]
        public void Test_Without_Spec_Item()
        {
            var db = UnitTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = new EntityResolver(db, null);

            var result = resolver.Resolve(new UserModel
            {
                OtherAccountWithoutSpec = new AccountBasicViewModel { Id = 1 }
            });

            (result.OtherAccountWithoutSpec as Account).Should().NotBeNull();
        }

        [TestMethod]
        public void Test_Collection_Items()
        {
            var db = UnitTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = new EntityResolver(db, null);

            var result = resolver.Resolve(new UserModel
            {
                ListAccounts = new[] {
                    new AccountBasicViewModel { Id = 1 },
                    new AccountBasicViewModel { Id = 2 } }
            });

            (result.ListAccounts.Count as int?).Should().Be(2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_ResolveAndMap_Error()
        {
            var db = UnitTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = new EntityResolver(db, null);
            var user = resolver.ResolveAndMap<User>(new UserModel
            {
                ListAccounts = new[] {
                    new AccountBasicViewModel { Id = 1 },
                    new AccountBasicViewModel { Id = 2 } }
            });
        }

        [TestMethod]
        public void Test_ResolveAndMap()
        {
            var db = UnitTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = UnitTestSetup.Provider.GetService<IEntityResolver>();

            var user = resolver.ResolveAndMap<User>(new UserModel
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
        public void Test_Resolve_Exception()
        {
            var db = UnitTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = UnitTestSetup.Provider.GetService<IEntityResolver>();

            var result = resolver.Resolve(new object());
            ((object)result).Should().BeNull();
        }

        [TestMethod]
        public void Test_Generic_ResolveAndMap_Exception()
        {
            var db = UnitTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = UnitTestSetup.Provider.GetService<IEntityResolver>();

            var result = resolver.ResolveAndMap<User>(new object());
            result.Should().BeNull();
        }

        [TestMethod]
        public void Test_ResolveAndMap_Exception()
        {
            var db = UnitTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = UnitTestSetup.Provider.GetService<IEntityResolver>();

            resolver.ResolveAndMap(new object(), new User());
        }

        [TestMethod]
        public void Test_AlwaysIncluded()
        {
            var db = UnitTestSetup.Db;
            db.SeedData().GetAwaiter().GetResult();

            var resolver = UnitTestSetup.Provider.GetService<IEntityResolver>();

            var rs = resolver.Resolve(new UserModel());
            (rs.CreateDateTime as DateTime?).Should().BeOnOrAfter(DateTime.Today);

            rs = resolver.Resolve(new UserModel(), true);
            (rs.CreateDateTime as DateTime?).Should().BeOnOrAfter(DateTime.Today);
        }
    }
}
