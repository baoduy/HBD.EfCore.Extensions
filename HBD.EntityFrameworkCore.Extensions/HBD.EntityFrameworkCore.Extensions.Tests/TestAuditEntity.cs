using DataLayer;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class TestAuditEntity
    {
        [TestMethod]
        public void TestCreatingEntity()
        {
            var user = new User("Duy");
            user.UpdatedByUser("Hoang");

            user.UpdatedBy.Should().BeNullOrEmpty();
            user.UpdatedOn.Should().BeNull();
        }

        [TestMethod]
        public void TestUpdatingEntity()
        {
            var user = new User(1,"Duy");
            user.UpdatedByUser("Hoang");

            user.UpdatedBy.Should().Be("Hoang");
            user.UpdatedOn.Should().NotBeNull();
        }
    }
}
