using FluentAssertions;
using HBD.EfCore.EntityResolver.Tests.Entities;
using HBD.EfCore.EntityResolver.Tests.Models;
using HBD.EfCore.EntityResolver.Tests.Specs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace HBD.EfCore.EntityResolver.Tests
{
    [TestClass]
    public class TestExtensions
    {
        [TestMethod]
        public void IsInstanceOf()
        {
            typeof(int).IsInstanceOf(typeof(IEquatable<>))
                .Should().BeTrue();

            typeof(int).IsInstanceOf(typeof(IEquatable<>))
                .Should().BeTrue();

            typeof(IEquatable<int>).IsInstanceOf(typeof(IEquatable<>))
                .Should().BeTrue();
        }


        [TestMethod]
        public void GetResolveInfo()
        {
            var info = Extensions.GetResolveInfo(typeof(UserModel)).ToList();

            info.Count.Should().BeGreaterOrEqualTo(2);
            info.All(i => i.Property != null &&
                          (i.AlwaysIncluded || i.Attribute?.EntityType != null)).Should().BeTrue();

            info.First().Attribute.EntityType.Should().Be(typeof(Account));

            var other = info.First(i => i.Property.Name == nameof(UserModel.OtherAccount));

            other.Attribute.EntityType.Should().Be(typeof(Account));
            other.Attribute.SpecType.Should().Be(typeof(AccountSpec));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetResolveInfo_Exception()
        {
            _ = Extensions.GetResolveInfo(typeof(UserErrorModel)).ToList();
        }

        [TestMethod]
        public void ToDynamic()
        {
            var info = new UserModel
            {
                AccountId = 1,
                OtherAccount = new AccountBasicViewModel { Id = 2 },
            };

            var dInfo = info.ToDynamic();

            (dInfo.AccountId as int?).Should().Be(info.AccountId);
            (dInfo.OtherAccount as AccountBasicViewModel).Should().Be(info.OtherAccount);

            dInfo.Account = new AccountBasicViewModel { Id = 1 };
            (dInfo.Account as AccountBasicViewModel)?.Id.Should().Be(info.AccountId);
        }

        [TestMethod]
        public void Get_Generic_Interfaces()
        {
            var types = typeof(UserModel).GetGenericInterfaceTypes().ToList();
            types.Should().HaveCount(1);
        }
    }
}
