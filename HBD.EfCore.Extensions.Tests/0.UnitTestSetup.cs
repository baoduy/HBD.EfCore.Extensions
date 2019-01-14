using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class UnitTestSetup
    {
        public static MyDbContext Db { get; private set; }

        [AssemblyInitialize]
        public static void TestSetup(TestContext _)
        {
            Db = new MyDbContext(new DbContextOptionsBuilder()
                 .UseSqliteMemory()
                 .UseLoggerFactory(SqliteMemory.DebugLoggerFactory)
                 .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
                 .Options);

            Db.Database.EnsureCreated();
        }

        [AssemblyCleanup]
        public static void TestCleanup()
        {
            Db?.Dispose();
        }
    }
}
