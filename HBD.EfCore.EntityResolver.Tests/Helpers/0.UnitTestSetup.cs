using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.EntityResolver.Tests.Helpers
{
    [TestClass]
    public class UnitTestSetup
    {
        public static MyDbContext Db { get; private set; }
        public static IServiceProvider Provider { get; private set; }

        [AssemblyInitialize]
        public static void TestSetup(TestContext _)
        {
            Provider = new ServiceCollection()
                .AddDbContext<MyDbContext>(op => op.UseSqliteMemory()
                    .UseLoggerFactory(SqliteMemory.DebugLoggerFactory)
                    .UseAutoConfigModel(i => i.ScanFrom(typeof(MyDbContext).Assembly)))
                .AddEntityResolver()
                .AddAutoMapper()
                .AddScoped<DbContext>(op => op.GetService<MyDbContext>())
                .BuildServiceProvider();

            Db = Provider.GetService<MyDbContext>();
            Db.Database.EnsureCreated();
        }

        [AssemblyCleanup]
        public static void TestCleanup()
        {
            Db?.Dispose();
        }
    }
}
