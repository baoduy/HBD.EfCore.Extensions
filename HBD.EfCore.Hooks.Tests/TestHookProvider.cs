using FluentAssertions;
using HBD.EfCore.Hooks.Tests.Entities;
using HBD.EfCore.Hooks.Tests.Providers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HBD.EfCore.Hooks.Tests
{
    [TestClass]
    public class TestHookProvider : TestBase
    {
        #region Public Methods

        [TestMethod]
        public void TestHook()
        {
            var db = GetService<TestHookDbContext>();
            var hooks = db.GetInfrastructure().GetServices<IHook>().ToList();

            db.Add(new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            });

            db.SaveChanges();

            hooks.Any(h => h is Dummy d && d.OnSavingCalled && d.OnSavedCalled).Should().BeTrue();
        }

        [TestMethod]
        public void TestHookAsync()
        {
            var db = GetService<TestHookDbContext>();
            var hooks = db.GetInfrastructure().GetServices<IHook>().ToList();

            db.Add(new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            });

            db.SaveChangesAsync();

            hooks.Any(h => h is Dummy d && d.OnSavingCalled && d.OnSavedCalled).Should().BeTrue();
        }

        #endregion Public Methods
    }
}