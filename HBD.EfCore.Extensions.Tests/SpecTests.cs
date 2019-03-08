using System;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DataLayer;
using FluentAssertions;
using HBD.EfCore.Extensions.Tests.Helpers;
using HBD.EfCore.Extensions.Tests.TestClasses;
using HBD.EfCore.Extensions.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class SpecTests
    {
        #region Public Methods

        [TestMethod]
        [Benchmark]
        public async Task TestPagingUser()
        {
            var list = await UnitTestSetup.Db.Set<User>().AsNoTracking().ToPageableAsync(
                new PageableSpec<User>(10, 100, u => u.FirstName,
                    spec: new UserIdGreaterThan10Spec()));

            list.Items.Count.Should().BeGreaterOrEqualTo(90);
            list.Items.All(u => u.Addresses.Count > 0).Should().BeTrue();
            list.Items.All(u => u.Account == null).Should().BeTrue();
        }

        [TestMethod]
        [Benchmark]
        public async Task TestSpecUser_Generic()
        {
            var list = await UnitTestSetup.Db.ForSpec<User, UserIdGreaterThan10Spec>().AsNoTracking().ToListAsync();
            list.Should().NotBeEmpty();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestPagingUser_NullOrderBy()
        {
            var list = await UnitTestSetup.Db.Set<User>().AsNoTracking().ToPageableAsync(
                new PageableSpec<User>(10, 100, null,
                    spec: new UserIdGreaterThan10Spec()));
        }

        [TestMethod]
        [Benchmark]
        public async Task TestUser_AndSpec()
        {
            var list = await UnitTestSetup.Db
                .ForSpec(new UserIdGreaterThan10Spec().And(new UserAccountStartWithDSpec()))
                .AsNoTracking()
                .ToListAsync();

            list.Should().NotBeEmpty();
            list.All(u => u.Addresses.Count > 0).Should().BeTrue();
            list.All(u => u.Account != null && u.Account.UserName.StartsWith("D")).Should().BeTrue();
            list.All(u => u.Id > 10).Should().BeTrue();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestUser_ButNotNullSpec()
        {
            var spec = new UserIdGreaterThan10Spec().ButNot(null);
            var list = await UnitTestSetup.Db.ForSpec(spec).AsNoTracking()
                .ToListAsync();
        }

        [TestMethod]
        [Benchmark]
        public async Task TestUser_ButNotSpec()
        {
            var spec = new UserIdGreaterThan10Spec().ButNot(new UserAccountStartWithDSpec());
            var list = await UnitTestSetup.Db.ForSpec(spec).AsNoTracking()
                .ToListAsync();

            list.Should().NotBeEmpty();
            list.All(u => u.Id > 10 && !u.Account.UserName.StartsWith("D")).Should().BeTrue();
        }

        [TestMethod]
        [Benchmark]
        public async Task TestUser_NotMeSpec()
        {
            await UnitTestSetup.Db.SeedData();

            var list = await UnitTestSetup.Db.ForSpec(new UserAccountStartWithDSpec().NotMe())
                .AsNoTracking()
                .Where(u => u.Account.UserName != null)
                .ToListAsync();

            list.Should().NotBeEmpty();
            list.All(u => u.Account.UserName.StartsWith("D")).Should().BeFalse();
        }

        [TestMethod]
        [Benchmark]
        public async Task TestUser_OrSpec()
        {
            var spec = new UserIdGreaterThan10Spec().Or(new UserAccountStartWithDSpec());

            var list = await UnitTestSetup.Db.ForSpec(spec).AsNoTracking()
                .ToListAsync();

            list.Should().NotBeEmpty();
            list.All(u => spec.IsSatisfied(u)).Should().BeTrue();
        }

        [TestMethod]
        public async Task TestUserSpecAsync_IncludingAccount()
        {
            await UnitTestSetup.Db.SeedData(force: true);

            var item = await UnitTestSetup.Db.ForSpec(new UserIncludeAccountSpec())
                .AsNoTracking().LastOrDefaultAsync();

            item.Should().NotBeNull();
            item.Account.Should().NotBeNull();
        }

        [TestMethod]
        public async Task TestUserSpecAsync_IncludingAddress()
        {
            var list = await UnitTestSetup.Db.ForSpec(new UserIdGreaterThan10Spec()).AsNoTracking().ToListAsync();

            list.Should().NotBeEmpty();
            list.All(u => u.Addresses.Count > 0).Should().BeTrue();
            list.All(u => u.Account == null).Should().BeTrue();
        }
        #endregion Public Methods
    }
}