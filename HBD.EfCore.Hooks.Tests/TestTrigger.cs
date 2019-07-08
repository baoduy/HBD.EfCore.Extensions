using FluentAssertions;
using HBD.EfCore.Hooks.Tests.Entities;
using HBD.EfCore.Hooks.Tests.Providers;
using HBD.EfCore.Hooks.Triggers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.EfCore.Hooks.Tests
{
    [TestClass]
    public class TestTrigger : TestBase
    {
        #region Public Methods

        [TestMethod]
        public void Test_Profiles_from_DI()
        {
            var profiles = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>().ToList();
            profiles.Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        public void Test_ScanFrom_Profile_Assemblies()
        {
            var interfaceType = typeof(ITriggerProfile);
            var profiles = TriggerExtensions.GetProfileTypes(new[] { typeof(Dummy).Assembly, typeof(Dummy).Assembly },
                interfaceType);

            profiles.Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        public void Trigger_Created_Event()
        {
            var db = GetService<TestHookDbContext>();

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;

            profile.Reset();

            var user = new User
            {
                FirstName = "Duy 1",
                LastName = "Hoang 1",
            };
            db.Add(user);

            db.SaveChanges();

            profile.Called.Should().Be(1);
        }

        [TestMethod]
        public async Task Trigger_Created_Event_Async()
        {
            var db = GetService<TestHookDbContext>();

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;

            profile.Reset();

            var user = new User
            {
                FirstName = "Duy 2",
                LastName = "Hoang 2",
            };
            db.Add(user);

            await db.SaveChangesAsync();

            profile.Called.Should().Be(1);
        }

        [TestMethod]
        public void Trigger_Delete_Event()
        {
            var db = GetService<TestHookDbContext>();

            var user = new User
            {
                FirstName = "Duy 3",
                LastName = "Hoang 3",
            };
            db.Add(user);

            db.SaveChanges();

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;

            profile.Reset();

            db.Remove(user);
            db.SaveChanges();

            profile.Called.Should().Be(1);
        }

        [TestMethod]
        public async Task Trigger_Delete_Event_Async()
        {
            var db = GetService<TestHookDbContext>();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            await db.SaveChangesAsync();

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;

            profile.Reset();

            db.Remove(user);
            await db.SaveChangesAsync();

            profile.Called.Should().Be(1);
        }

        [TestMethod]
        public async Task Trigger_Disabled_Async()
        {
            TriggerHook.Disabled = true;

            var db = GetService<TestHookDbContext>();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            await db.SaveChangesAsync();

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;

            profile.Reset();

            db.Remove(user);
            await db.SaveChangesAsync();

            profile.Called.Should().Be(0);

            TriggerHook.Disabled = false;
        }

        [TestMethod]
        public void Trigger_ServiceProvider_NotNull()
        {
            var db = GetService<TestHookDbContext>();

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;

            profile.Reset();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            db.SaveChanges();

            profile.Called.Should().Be(1);
            profile.HasEntity.Should().BeTrue();
            profile.HasServiceProvider.Should().BeTrue();
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

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;

            profile.Reset();

            user.FirstName = "AAA";
            db.SaveChanges();

            profile.Called.Should().Be(1);
            profile.HasFirstNameChanged.Should().BeTrue();
        }

        [TestMethod]
        public async Task Trigger_Updated_Event_Async()
        {
            var db = GetService<TestHookDbContext>();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            await db.SaveChangesAsync();

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is DummyTriggerProfile) as DummyTriggerProfile;

            profile.Reset();

            user.FirstName = "AAA";

            await db.SaveChangesAsync();

            profile.Called.Should().Be(1);
            profile.HasFirstNameChanged.Should().BeTrue();
        }


        [TestMethod]
        public void Hook_ShouldNot_Be_Called_If_CollectionIs_Empty_AddNew()
        {
            var db = GetService<TestHookDbContext>();

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is UpdatedTriggerProfile) as UpdatedTriggerProfile;

            profile.Reset();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };

            db.Add(user);

            db.SaveChanges();

            profile.IsCalled.Should().BeFalse();
        }

        [TestMethod]
        public void Hook_ShouldNot_Be_Called_If_CollectionIs_Empty_Update()
        {
            Hook_ShouldNot_Be_Called_If_CollectionIs_Empty_AddNew();

            var db = GetService<TestHookDbContext>();

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is UpdatedTriggerProfile) as UpdatedTriggerProfile;

            profile.Reset();

            var user = db.Set<User>().Last();

            user.FirstName = "Updated Lah lah";

            db.SaveChanges();

            profile.IsCalled.Should().BeTrue();
            profile.IsEmpty.Should().BeFalse();
        }
        #endregion Public Methods
    }
}