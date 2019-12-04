using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HBD.TestHelper
{
    public class UnitTestSetup
    {
        #region Fields

        private static MyDbContext db;
        private static IServiceProvider provider;

        #endregion Fields

        #region Properties

        public static MyDbContext Db => db ??= Initialize().db;

        public static IServiceProvider Provider => provider ??= Initialize().provider;

        #endregion Properties

        #region Methods

        public static (MyDbContext db, IServiceProvider provider) Initialize()
        {
            provider = new ServiceCollection()
              .AddDbContext<MyDbContext>(op => op.UseSqliteMemory()
                  .UseLoggerFactory(SqliteMemory.DebugLoggerFactory)
                  .UseAutoConfigModel(i => i.ScanFrom(typeof(MyDbContext).Assembly)))
              .AddScoped<DbContext>(op => op.GetService<MyDbContext>())
              .BuildServiceProvider();

            db = Provider.GetService<MyDbContext>();
            db.Database.EnsureCreated();

            return (db, provider);
        }

        #endregion Methods
    }
}