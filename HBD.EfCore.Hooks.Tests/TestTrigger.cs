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
        #region Methods

        [TestMethod]
        public void Hook_ShouldNot_Be_Called_If_CollectionIs_Empty_AddNew()
        {
            var db = GetService<TestHookDbContext>();

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is UserUpdatedTrigger) as UserUpdatedTrigger;

            profile.Reset();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };

            db.Add(user);
            db.SaveChanges();

            profile.CalledCount.Should().Be(0);
        }

        [TestMethod]
        public void Hook_ShouldNot_Be_Called_If_CollectionIs_Empty_Update()
        {
            Hook_ShouldNot_Be_Called_If_CollectionIs_Empty_AddNew();

            var db = GetService<TestHookDbContext>();

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is UserUpdatedTrigger) as UserUpdatedTrigger;

            profile.Reset();

            var user = db.Set<User>().First();
            user.FirstName = "Updated Lah lah";

            db.SaveChanges();

            profile.CalledCount.Should().Be(1);
            profile.HasEntity.Should().BeTrue();
        }

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
            var triggerAll = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .OfType<UserTrigger>().ToList();

            triggerAll.ResetAll();

            var user = new User
            {
                FirstName = "Duy 1",
                LastName = "Hoang 1",
            };

            db.Add(user);
            db.SaveChanges();

            triggerAll.ForEach(t =>
            {
                if (t is UserTriggerAll a)
                {
                    a.CalledCount.Should().Be(1);
                    a.HasEntity.Should().BeTrue();
                }
                else if (t is UserCreatedTrigger c)
                {
                    c.CalledCount.Should().Be(1);
                    c.HasEntity.Should().BeTrue();
                }
                else
                {
                    t.CalledCount.Should().Be(0);
                    t.HasEntity.Should().BeFalse();
                }
            });
        }

        [TestMethod]
        public async Task Trigger_Created_Event_Async()
        {
            var db = GetService<TestHookDbContext>();

            var triggerAll = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .OfType<UserTrigger>().ToList();

            triggerAll.ResetAll();

            var user = new User
            {
                FirstName = "Duy 2",
                LastName = "Hoang 2",
            };
            db.Add(user);

            await db.SaveChangesAsync().ConfigureAwait(false);

            triggerAll.ForEach(t =>
            {
                if (t is UserTriggerAll a)
                {
                    a.CalledCount.Should().Be(1);
                    a.HasEntity.Should().BeTrue();
                }
                else if (t is UserCreatedTrigger c)
                {
                    c.CalledCount.Should().Be(1);
                    c.HasEntity.Should().BeTrue();
                }
                else
                {
                    t.CalledCount.Should().Be(0);
                    t.HasEntity.Should().BeFalse();
                }
            });
        }

        [TestMethod]
        public void Trigger_Delete_Event()
        {
            var db = GetService<TestHookDbContext>();
            var triggerAll = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
               .OfType<UserTrigger>().ToList();

            var user = new User
            {
                FirstName = "Duy 3",
                LastName = "Hoang 3",
            };
            db.Add(user);
            db.SaveChanges();

            triggerAll.ResetAll();

            db.Remove(user);
            db.SaveChanges();

            triggerAll.ForEach(t =>
            {
                if (t is UserTriggerAll a)
                {
                    a.CalledCount.Should().Be(1);
                    a.HasEntity.Should().BeTrue();
                }
                else if (t is UserDeletedTrigger c)
                {
                    c.CalledCount.Should().Be(1);
                    c.HasEntity.Should().BeTrue();
                }
                else
                {
                    t.CalledCount.Should().Be(0);
                    t.HasEntity.Should().BeFalse();
                }
            });
        }

        [TestMethod]
        public async Task Trigger_Delete_Event_Async()
        {
            var db = GetService<TestHookDbContext>();
            var triggerAll = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
              .OfType<UserTrigger>().ToList();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            await db.SaveChangesAsync().ConfigureAwait(false);

            triggerAll.ResetAll();

            db.Remove(user);
            await db.SaveChangesAsync().ConfigureAwait(false);

            triggerAll.ForEach(t =>
            {
                if (t is UserTriggerAll a)
                {
                    a.CalledCount.Should().Be(1);
                    a.HasEntity.Should().BeTrue();
                }
                else if (t is UserDeletedTrigger c)
                {
                    c.CalledCount.Should().Be(1);
                    c.HasEntity.Should().BeTrue();
                }
                else
                {
                    t.CalledCount.Should().Be(0);
                    t.HasEntity.Should().BeFalse();
                }
            });
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

            await db.SaveChangesAsync().ConfigureAwait(false);

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is UserTriggerAll) as UserTriggerAll;

            profile.Reset();

            db.Remove(user);
            await db.SaveChangesAsync().ConfigureAwait(false);

            profile.CalledCount.Should().Be(0);

            TriggerHook.Disabled = false;
        }

        [TestMethod]
        public void Trigger_ServiceProvider_NotNull()
        {
            var db = GetService<TestHookDbContext>();

            var profile = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
                .First(i => i is UserTriggerAll) as UserTriggerAll;

            profile.Reset();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            db.SaveChanges();

            profile.CalledCount.Should().Be(1);
            profile.HasEntity.Should().BeTrue();
            profile.HasServiceProvider.Should().BeTrue();
        }

        [TestMethod]
        public void Trigger_Updated_Event()
        {
            var db = GetService<TestHookDbContext>();
            var triggerAll = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
             .OfType<UserTrigger>().ToList();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            db.SaveChanges();

            triggerAll.ResetAll();

            user.FirstName = "AAA";
            db.SaveChanges();

            triggerAll.ForEach(t =>
            {
                if (t is UserTriggerAll a)
                {
                    a.CalledCount.Should().Be(1);
                    a.HasEntity.Should().BeTrue();
                }
                else if (t is UserUpdatedTrigger c)
                {
                    c.CalledCount.Should().Be(1);
                    c.HasEntity.Should().BeTrue();
                }
                else
                {
                    t.CalledCount.Should().Be(0);
                    t.HasEntity.Should().BeFalse();
                }
            });
        }

        [TestMethod]
        public async Task Trigger_Updated_Event_Async()
        {
            var db = GetService<TestHookDbContext>();
            var triggerAll = TriggerHook.Context.ServiceProvider.GetServices<ITriggerProfile>()
            .OfType<UserTrigger>().ToList();

            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
            };
            db.Add(user);

            await db.SaveChangesAsync().ConfigureAwait(false);

            triggerAll.ResetAll();

            user.FirstName = "AAA";

            await db.SaveChangesAsync().ConfigureAwait(false);

            triggerAll.ForEach(t =>
            {
                if (t is UserTriggerAll a)
                {
                    a.CalledCount.Should().Be(1);
                    a.HasEntity.Should().BeTrue();
                }
                else if (t is UserUpdatedTrigger c)
                {
                    c.CalledCount.Should().Be(1);
                    c.HasEntity.Should().BeTrue();
                }
                else
                {
                    t.CalledCount.Should().Be(0);
                    t.HasEntity.Should().BeFalse();
                }
            });
        }

        #endregion Methods
    }
}