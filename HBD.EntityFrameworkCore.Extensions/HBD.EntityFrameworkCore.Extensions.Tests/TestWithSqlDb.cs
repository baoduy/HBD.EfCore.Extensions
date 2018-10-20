using System.Linq;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class TestWithSqlDb
    {
        private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True;Connect Timeout=30;";

        [TestMethod]
        public async System.Threading.Tasks.Task TestAsync()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqlServer(ConnectionString)
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
                .Options))
            {
                await db.Database.EnsureCreatedAsync();

                //Create User with Address
                await db.Set<User>().AddAsync(new User("Duy")
                {
                    FirstName = "Duy",
                    LastName = "Hoang",
                    Addresses = {
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
    }
}
