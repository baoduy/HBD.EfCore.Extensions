using DataLayer;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class AuditEntityTests
    {
        #region Public Methods

        [TestMethod]
        public void TestCreatingEntity()
        {
            var user = new User("Duy");
            user.UpdatedByUser("Hoang");

            user.UpdatedBy.Should().BeNullOrEmpty();
            user.UpdatedOn.Should().BeNull();
            user.Id.Should().Be(0);
        }

        [TestMethod]
        public void TestUpdatingEntity()
        {
            var user = new User(1, "Duy");
            user.UpdatedByUser("Hoang");

            user.UpdatedBy.Should().Be("Hoang");
            user.UpdatedOn.Should().NotBeNull();
            user.Id.Should().Be(1);
        }

        #endregion Public Methods
    }
}