using System;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DataLayer;
using FluentAssertions;
using HBD.EntityFrameworkCore.Extensions.Specification;
using HBD.EntityFrameworkCore.Extensions.Tests.TestClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class TestSpec
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
            var list = await UnitTestSetup.Db.ForSpec(new UserAccountStartWithDSpec().NotMe())
                .AsNoTracking()
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
            var list = await UnitTestSetup.Db.ForSpec(new UserIncludeAccountSpec()).Where(u => u.Id < 5).ToListAsync();

            list.Should().NotBeEmpty();
            list.All(u => u.Account != null).Should().BeTrue();
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