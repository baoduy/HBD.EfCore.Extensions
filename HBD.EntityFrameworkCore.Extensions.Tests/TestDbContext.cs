using System.Threading;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class TestDbContext
    {
        [TestMethod]
        public async Task Ensure_OnSaving_Called()
        {
            var dbMoq = new Mock<DbContext>(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .UseLoggerFactory(SqliteMemory.DebugLoggerFactory)
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
                .Options)
            { CallBase = true };

            dbMoq.Protected().Setup("OnSaving").Verifiable();

            var db = dbMoq.Object;

            db.SaveChanges();
            db.SaveChanges(true);
            await db.SaveChangesAsync();
            await db.SaveChangesAsync(new CancellationToken(false));
            await db.SaveChangesAsync(true);

            dbMoq.Protected().Verify("OnSaving", Times.AtLeast(5));
        }
    }
}
