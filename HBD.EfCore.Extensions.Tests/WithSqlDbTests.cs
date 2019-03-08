using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class WithSqlDbTests
    {
        #region Private Fields

        private const string ConnectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True;Connect Timeout=30;";

        #endregion Private Fields

        #region Public Methods

        [TestMethod]
        public async Task TestAsync()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqlServer(ConnectionString)
                .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly))
                .Options))
            {
                await db.Database.EnsureCreatedAsync();

                //Create User with Address
                await db.Set<User>().AddAsync(new User("Duy")
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

                await db.SaveChangesAsync();

                var users = await db.Set<User>().ToListAsync();

                await db.Database.EnsureDeletedAsync();

                Assert.IsTrue(users.Count == 1);

                Assert.IsTrue(users.All(u => u.RowVersion != null));
            }
        }

        #endregion Public Methods
    }
}