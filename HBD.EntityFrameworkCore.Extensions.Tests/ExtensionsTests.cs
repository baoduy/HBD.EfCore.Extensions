using System;
using DataLayer;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        #region Public Methods

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

        #endregion Public Methods
    }
}