using System;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using FluentAssertions;
using HBD.EfCore.Extensions.Tests.Helpers;
using HBD.EfCore.Extensions.Tests.TestClasses;
using HBD.EfCore.Extensions.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Remotion.Linq.Clauses;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class TestPageable
    {
        #region Public Methods
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Tes_WrongPageIndex()
        {
            var result = await UnitTestSetup.Db.Set<User>().OrderBy(u => u.Id).ToPageableAsync(-1, 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Tes_WrongPageSize()
        {
            var result = await UnitTestSetup.Db.Set<User>().OrderBy(u => u.Id).ToPageableAsync(0, 0);
        }

        [TestMethod]
        public async Task TesAsync()
        {
            await UnitTestSetup.Db.SeedData();

            var result = await UnitTestSetup.Db.Set<User>().OrderBy(u => u.Id).ToPageableAsync(0, 100);

            result.Items.Should().HaveCount(100);
            result.PageIndex.Should().Be(0);
            result.PageSize.Should().Be(100);
            result.TotalItems.Should().BeGreaterOrEqualTo(100);
            result.TotalPage.Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        public async Task Test()
        {
            await UnitTestSetup.Db.SeedData();

            var result = UnitTestSetup.Db.Set<User>().OrderBy(u => u.Id).ToPageable(0, 10);

            result.Items.Should().HaveCount(10);
            result.PageIndex.Should().Be(0);
            result.PageSize.Should().Be(10);
            result.TotalItems.Should().BeGreaterOrEqualTo(100);
            result.TotalPage.Should().BeGreaterOrEqualTo(10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ForPageSpec_Misuse()
        {
            var result = UnitTestSetup.Db.ForSpec(new PageableSpec<User>(1, 10, i => i.FirstName));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForPageSpec_Null()
        {
            var result = UnitTestSetup.Db.ForPageSpec((PageableSpec<User>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ForPageSpecAsync_NullAsync()
        {
            var result = await UnitTestSetup.Db.ForPageSpecAsync((PageableSpec<User>)null);
        }

        [TestMethod]
        public async Task ForPageSpec()
        {
            await UnitTestSetup.Db.SeedData();

            var result = UnitTestSetup.Db.ForPageSpec(new PageableSpec<User>(1, 10, i => i.FirstName));

            result.Items.Should().HaveCount(10);
            result.PageIndex.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalItems.Should().BeGreaterOrEqualTo(100);
            result.TotalPage.Should().BeGreaterOrEqualTo(10);
        }

        [TestMethod]
        public async Task ForPageSpecAsync()
        {
            await UnitTestSetup.Db.SeedData();

            var result = await UnitTestSetup.Db.ForPageSpecAsync(new PageableSpec<User>(1, 10, i => i.FirstName));

            result.Items.Should().HaveCount(10);
            result.PageIndex.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalItems.Should().BeGreaterOrEqualTo(100);
            result.TotalPage.Should().BeGreaterOrEqualTo(10);
        }

        [TestMethod]
        public async Task ForPageSpecAsync_Desc()
        {
            await UnitTestSetup.Db.SeedData();

            var result = await UnitTestSetup.Db.ForPageSpecAsync(new PageableSpec<User>(1, 10, i => i.FirstName, OrderingDirection.Desc));

            result.Items.Should().HaveCount(10);
            result.PageIndex.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalItems.Should().BeGreaterOrEqualTo(100);
            result.TotalPage.Should().BeGreaterOrEqualTo(10);
        }

        [TestMethod]
        public async Task ForPageSpecAsync_WithSpec()
        {
            await UnitTestSetup.Db.SeedData();

            var result = await UnitTestSetup.Db.ForPageSpecAsync(new PageableSpec<User>(1, 10, i => i.FirstName, spec: new UserIdGreaterThan10Spec()));

            result.Items.Should().HaveCount(10);
            result.Items.All(u => u.Id > 10).Should().BeTrue();
        }
        #endregion Public Methods
    }
}