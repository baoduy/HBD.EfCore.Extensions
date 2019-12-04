using AutoMapper;
using HBD.EfCore.DDD.Tests.Infra;
using HBD.TestHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            Provider = new ServiceCollection()
                .AddLogging()
                .AddAutoMapper(typeof(Initialize).Assembly)
                .AddDomainWithSingleBoundedContext<ProfileContext>(b =>
                {
                    b.UseSqliteMemory();
                },

                //eventConfig: new GenericEventRunnerConfig { NotUsingAfterSaveHandlers = false },
                assembliesToScans: typeof(Initialize).Assembly)
                .BuildServiceProvider();

            var db = Provider.GetRequiredService<ProfileContext>();
            db.Database.EnsureCreated();
        }

        #endregion Methods
    }
}