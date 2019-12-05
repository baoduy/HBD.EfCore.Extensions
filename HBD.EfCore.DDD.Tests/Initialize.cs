using AutoBogus;
using AutoBogus.Conventions;
using HBD.EfCore.DDD.Tests.Infra;
using HBD.TestHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.DDD.Tests
{
    [TestClass]
    public class Initialize
    {
        #region Properties

        public static IServiceProvider Provider { get; private set; }

        #endregion Properties

        #region Methods

        [AssemblyInitialize]
        public static void Setup(TestContext _)
        {
            AutoFaker.Configure(builder =>
            {
                builder.WithConventions();
            });

            Provider = new ServiceCollection()
                .AddLogging()
                .AddDomainWithSingleBoundedContext<ProfileContext>(b =>
                {
                    b.UseSqliteMemory();
                },

                //eventConfig: new GenericEventRunnerConfig { NotUsingAfterSaveHandlers = false },
                assembliesToScans: typeof(Initialize).Assembly)
                .AddScoped<DbContext>(p => p.GetRequiredService<ProfileContext>())
                .BuildServiceProvider();

            var db = Provider.GetRequiredService<ProfileContext>();
            db.Database.EnsureCreated();
            db.Generate().GetAwaiter().GetResult();
        }

        #endregion Methods

        [TestMethod]
        public void TestInitialize()
        {
            var db = Initialize.Provider.GetService<ProfileContext>();
            db.Set<Infra.Profile>().All(p => p.Accounts.Count == 100)
                .Should().BeTrue();
        }
    }
}