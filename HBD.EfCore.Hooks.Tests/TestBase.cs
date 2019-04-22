using System;
using HBD.EfCore.Hooks.Tests.Providers;
using HBD.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Hooks.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        private IServiceProvider _provider;

        [TestInitialize]
        public void Setup()
        {
            var coll = new ServiceCollection()
                .AddDbContextWithHooks<TestHookDbContext>(
                    hooks => hooks.UseTrigger().ScanFrom(typeof(Dummy).Assembly),
                    op =>
                        op.UseSqliteMemory()
                            .UseDebugLogger());

            _provider = coll.BuildServiceProvider();
            var db = GetService<TestHookDbContext>();
            db.Database.EnsureCreated();
        }

        protected T GetService<T>()
            => _provider.GetService<T>();
    }
}
