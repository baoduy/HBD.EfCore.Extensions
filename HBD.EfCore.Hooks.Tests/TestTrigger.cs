using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HBD.EfCore.Hooks.Tests.Entities;
using HBD.EfCore.Hooks.Tests.Providers;
using HBD.EfCore.Hooks.Triggers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Hooks.Tests
{
    [TestClass]
    public class TestTrigger : TestBase
    {
        [TestMethod]
        public void Trigger_ServiceProvider_NotNull()
        {
            var db = GetService<TestHookDbContext>();

            var profile = db.GetInfrastructure().GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;
            profile.Reset();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            db.SaveChanges();

            Thread.Sleep(10);
            profile.HasEntity.Should().BeTrue();
            profile.HasServiceProvider.Should().BeTrue();
        }

        [TestMethod]
        public void Trigger_Created_Event()
        {
            var db = GetService<TestHookDbContext>();

            var profile = db.GetInfrastructure().GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;
            profile.Reset();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            db.SaveChanges();

            Thread.Sleep(10);
            profile.Called.Should().BeTrue();
        }

        [TestMethod]
        public async Task Trigger_Created_Event_Async()
        {
            var db = GetService<TestHookDbContext>();

            var profile = db.GetInfrastructure().GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;
            profile.Reset();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            await db.SaveChangesAsync();

            await Task.Delay(10);
            profile.Called.Should().BeTrue();
        }

        [TestMethod]
        public void Trigger_Updated_Event()
        {
            var db = GetService<TestHookDbContext>();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            db.SaveChanges();

            var profile = db.GetInfrastructure().GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;
            profile.Reset();

            user.FirstName = "AAA";
            db.SaveChanges();

            Thread.Sleep(10);
            profile.Called.Should().BeTrue();
        }

        [TestMethod]
        public async Task Trigger_Updated_Event_Async()
        {
            var db = GetService<TestHookDbContext>();
            var called = false;

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            await db.SaveChangesAsync();
          
            var profile = db.GetInfrastructure().GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;
            profile.Reset();

            user.FirstName = "AAA";

            await db.SaveChangesAsync();

            await Task.Delay(10);
            profile.Called.Should().BeTrue();
        }

        [TestMethod]
        public void Trigger_Delete_Event()
        {
            var db = GetService<TestHookDbContext>();
            var called = false;

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            db.SaveChanges();

            var profile = db.GetInfrastructure().GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;
            profile.Reset();

            db.Remove(user);
            db.SaveChanges();

            Thread.Sleep(10);
            profile.Called.Should().BeTrue();
        }

        [TestMethod]
        public async Task Trigger_Delete_Event_Async()
        {
            var db = GetService<TestHookDbContext>();
            var called = false;

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            await db.SaveChangesAsync();

            var profile = db.GetInfrastructure().GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;
            profile.Reset();

            db.Remove(user);
            await db.SaveChangesAsync();

            await Task.Delay(10);
            profile.Called.Should().BeTrue();
        }
    }
}
