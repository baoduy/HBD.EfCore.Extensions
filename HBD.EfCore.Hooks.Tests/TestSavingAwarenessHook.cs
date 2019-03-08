using FluentAssertions;
using HBD.EfCore.Hooks.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Hooks.Tests
{
    [TestClass]
    public class TestSavingAwarenessHook : TestBase
    {
        #region Public Methods

        [TestMethod]
        public void TestSavingAwareness()
        {
            var db = GetService<TestHookDbContext>();
            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            db.SaveChanges();

            user.SavingCalled.Should().Be(1);
        }

        [TestMethod]
        public void TestSavingAwarenessAsync()
        {
            var db = GetService<TestHookDbContext>();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            db.SaveChangesAsync();

            user.SavingCalled.Should().Be(1);
        }

        #endregion Public Methods
    }
}