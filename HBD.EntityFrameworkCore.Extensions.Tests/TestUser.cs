using DataLayer;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class TestUser
    {
        #region Public Methods

        [TestMethod]
        public void AddUserAndAddress()
        {
            var user = new User("A")
            {
                FirstName = "Duy",
                LastName = "Hoang",
                Addresses =
                {
                    new Address
                    {
                        OwnedEntity = new OwnedEntity
                        {
                            Name = "A"
                        },
                        Street = "123",
                    },
                    new Address
                    {
                        OwnedEntity = new OwnedEntity
                        {
                            Name = "B"
                        },
                        Street = "124",
                    }
                }
            };

            UnitTestSetup.Db.Add(user);
            UnitTestSetup.Db.SaveChanges();

            user.SavingCalled.Should().BeGreaterOrEqualTo(1);

            var u = UnitTestSetup.Db.Set<User>().Last();
            u.Should().NotBeNull();
            u.Addresses.Should().HaveCount(2);

            u.Addresses.Remove(u.Addresses.Last());
            UnitTestSetup.Db.SaveChanges();

            u = UnitTestSetup.Db.Set<User>().Last();
            u.Addresses.Should().HaveCount(1);
        }

        #endregion Public Methods
    }
}