using DataLayer;
using HBD.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Extensions.Tests.Helpers
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
                 .UseDebugLogger()
                 .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly))
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
