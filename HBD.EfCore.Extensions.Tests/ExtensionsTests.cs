using DataLayer;
using DataLayer.Specs;
using FluentAssertions;
using HBD.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        #region Methods

        [TestMethod]
        public void Test_GetKeys()
        {
            UnitTestSetup.Db.GetKeys<User>().Single()
                .Should().Be("Id");
        }

        [TestMethod]
        public void Test_GetKeys_NotEntity()
        {
            UnitTestSetup.Db.GetKeys<UserAccountStartWithDSpec>().Any()
                .Should().BeFalse();
        }

        [TestMethod]
        public void Test_GetKeyValue()
        {
            var user = new User(1, "Duy");
            UnitTestSetup.Db.GetKeyValuesOf<User>(user).Single()
                .Should().Be(1);
        }

        [TestMethod]
        public void Test_GetKeyValue_NotEntity()
        {
            var user = new { Id = 1, Name = "Duy" };
            UnitTestSetup.Db.GetKeyValuesOf<object>(user).Any()
                .Should().BeFalse();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNUll_UpdateFrom()
        {
            ((object)null).UpdateFrom("A");
        }

        [TestMethod]
        public void TestUpdateFrom()
        {
            var user = new OwnedEntity { Name = "Duy" };
            var user1 = new OwnedEntity { Name = "Steven" };

            user.UpdateFrom(user1);

            user.Name.Should().Be(user1.Name);
        }

        [TestMethod]
        public void TestUpdateFrom_IgnoreNull()
        {
            var user = new OwnedEntity { Name = "Duy" };
            var user1 = new OwnedEntity { Name = null };

            user1.UpdateFrom(user1, true);

            user.Name.Should().Be("Duy");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUpdateFrom_NotOwnedType()
        {
            var user = new User("Duy");
            var user1 = new User("Steven")
            {
                FirstName = "Duy",
                LastName = "Hoang",
                Account = new Account()
            };

            user.UpdateFrom(user1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestUpdateFrom_Null()
        {
            var user = new User("Duy");
            user.UpdateFrom(null);
        }

        [TestMethod]
        public void TestUpdateFrom_ReadOnly()
        {
            var user = new OwnedEntity { Name = "Duy", ReadOnly = "A", NotReadOnly = "B" };
            var user1 = new OwnedEntity { Name = "Steven", ReadOnly = "B", NotReadOnly = "C" };

            user.UpdateFrom(user1);

            user.Name.Should().Be(user1.Name);
            user.ReadOnly.Should().Be("A");
            user.NotReadOnly.Should().Be(user1.NotReadOnly);
        }

        #endregion Methods
    }
}