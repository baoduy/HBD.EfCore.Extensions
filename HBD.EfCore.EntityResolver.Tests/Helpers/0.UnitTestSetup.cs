using System;
using AutoMapper;
using HBD.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.EntityResolver.Tests.Helpers
{
    [TestClass]
    public class ResolverTestSetup
    {
        #region Properties

        public static MyDbContext Db { get; private set; }

        public static IServiceProvider Provider { get; private set; }

        #endregion Properties

        #region Methods

        [AssemblyCleanup]
        public static void TestCleanup()
        {
            Db?.Dispose();
        }

        [AssemblyInitialize]
        public static void TestSetup(TestContext _)
        {
            Provider = new ServiceCollection()
                .AddDbContext<MyDbContext>(op => op.UseSqliteMemory()
                    .UseLoggerFactory(SqliteMemory.DebugLoggerFactory)
                    .UseAutoConfigModel(i => i.ScanFrom(typeof(MyDbContext).Assembly)))
                .AddEntityResolver()
                .AddAutoMapper(typeof(MyDbContext).Assembly)
                .AddScoped<DbContext>(op => op.GetService<MyDbContext>())
                .BuildServiceProvider();

            Db = Provider.GetService<MyDbContext>();
            Db.Database.EnsureCreated();
        }

        #endregion Methods
    }
}