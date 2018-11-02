using Bogus;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class TestPageable
    {
        [TestMethod]
        public void Test()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
                .Options))
            {
                db.Database.EnsureCreated();

                var users = new Faker<User>()
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.CreatedBy, f => f.Name.FirstName())
                    .RuleFor(u => u.UpdatedBy, f => f.Name.FirstName())
                    .RuleFor(u => u.CreatedOn, f => f.Date.SoonOffset())
                    .RuleFor(u => u.UpdatedOn, f => f.Date.SoonOffset())
                    .Generate(1000);

                db.AddRange(users);
                db.SaveChanges();

               var result= db.Set<User>().OrderBy(u => u.Id).ToPageable(0, 10);

                Assert.IsTrue(result.Items.Count==10);
                Assert.IsTrue(result.PageIndex == 0);
                Assert.IsTrue(result.PageSize == 10);
                Assert.IsTrue(result.TotalItems == 1000);
                Assert.IsTrue(result.TotalPage == 100);
            }
        }

        [TestMethod]
        public async Task TesAsync()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
                .Options).LogToConsole())
            {
               await db.Database.EnsureCreatedAsync();

                var users = new Faker<User>()
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.CreatedBy, f => f.Name.FirstName())
                    .RuleFor(u => u.UpdatedBy, f => f.Name.FirstName())
                    .RuleFor(u => u.CreatedOn, f => f.Date.SoonOffset())
                    .RuleFor(u => u.UpdatedOn, f => f.Date.SoonOffset())
                    .GenerateLazy(1000);

                await db.AddRangeAsync(users);
                await db.SaveChangesAsync();

                var result =await db.Set<User>().OrderBy(u => u.Id).ToPageableAsync(0, 100);

                Assert.IsTrue(result.Items.Count == 100);
                Assert.IsTrue(result.PageIndex == 0);
                Assert.IsTrue(result.PageSize == 100);
                Assert.IsTrue(result.TotalItems == 1000);
                Assert.IsTrue(result.TotalPage == 10);
            }
        }
    }
}
