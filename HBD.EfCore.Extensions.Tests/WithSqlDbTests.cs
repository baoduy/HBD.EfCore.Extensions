using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class WithSqlDbTests
    {
        #region Fields

        private MyDbContext db;

        #endregion Fields

        #region Properties

        public static string ConnectionString =>
                     RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hbd.EfCore.Test;Integrated Security=True;Connect Timeout=30;" :
            "Data Source=localhost;Initial Catalog=CodesERP;User Id=sa;Password=Pass@word1;";

        #endregion Properties

        #region Methods

        [TestCleanup]
        public void CleanUp()
        {
            db.Database.EnsureDeleted();
            db.Dispose();
        }

        [TestInitialize]
        public void Setup()
        {
            db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqlServer(ConnectionString)
                .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly))
                .Options);

            db.Database.Migrate();
        }

        [TestMethod]
        public async Task Test_Create_WithSqlDb_Async()
        {
            //Create User with Address
            db.Set<User>().Add(new User("Duy")
            {
                FirstName = "Duy",
                LastName = "Hoang",
                Addresses =
                    {
                        new Address
                        {
                            Street = "12"
                        }
                    }
            });

            var count = await db.SaveChangesAsync().ConfigureAwait(false);
            Assert.IsTrue(count >= 1);

            var users = await db.Set<User>().ToListAsync().ConfigureAwait(false);

            Assert.IsTrue(users.Count >= 1);
            Assert.IsTrue(users.All(u => u.RowVersion != null));
        }

        [TestMethod]
        public async Task Test_Delete_WithSqlDb_Async()
        {
            await Test_Create_WithSqlDb_Async().ConfigureAwait(false);

            var user = await db.Set<User>().FirstAsync().ConfigureAwait(false);

            db.Remove(user);

            await db.SaveChangesAsync().ConfigureAwait(false);

            var count = await db.Set<User>().CountAsync(u => u.Id == user.Id).ConfigureAwait(false);

            Assert.IsTrue(count == 0);
        }

        [TestMethod]
        public async Task Test_Update_WithSqlDb_Async()
        {
            await Test_Create_WithSqlDb_Async().ConfigureAwait(false);

            var user = await db.Set<User>().FirstAsync().ConfigureAwait(false);

            user.FirstName = "Steven";
            user.Addresses.Last().Street = "Steven Street";

            await db.SaveChangesAsync().ConfigureAwait(false);

            user = await db.Set<User>().FirstAsync().ConfigureAwait(false);

            Assert.IsTrue(user.FirstName == "Steven");

            Assert.IsTrue(user.Addresses.Last().Street == "Steven Street");
        }

        #endregion Methods
    }
}