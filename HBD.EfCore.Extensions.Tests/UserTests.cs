using DataLayer;
using FluentAssertions;
using HBD.EfCore.Extensions.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class UserTests
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

            var u = UnitTestSetup.Db.Set<User>().Last();
            u.Should().NotBeNull();
            u.Addresses.Should().HaveCount(2);

            u.Addresses.Remove(u.Addresses.Last());
            UnitTestSetup.Db.SaveChanges();

            u = UnitTestSetup.Db.Set<User>().Last();
            u.Addresses.Should().HaveCount(1);

            UnitTestSetup.Db.ChangeTracker.AutoDetectChangesEnabled.Should().BeTrue();
        }

        #endregion Public Methods
    }
}