using System;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        private async Task AddData(DbContext dbContext)
        {
            await dbContext.Database.EnsureCreatedAsync();

            //Create User with Address
            await dbContext.Set<User>().AddAsync(new User
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

            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Address_Is_InUse()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
                .Options))
            {
                db.LogToConsole();
                await AddData(db);

                var address = db.Set<Address>().First();
                //Referencing by User
                db.IsEntityInUse(address).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task User_IsNot_InUse()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
                .Options))
            {
                db.LogToConsole();
                await AddData(db);

                var u = db.Set<User>().First();
                //User is not referencing by Address
                db.IsEntityInUse(u).Should().BeFalse();
            }
        }

        [TestMethod]
        public async Task User_IsNot_InUse_WithOneToOne()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
                .Options))
            {
                db.LogToConsole();
                await AddData(db);

                var u = db.Set<User>().First();
                var account = new Account { UserName = "1", Password = "1" };
                u.Account = account;
                account.User = u;

                await db.SaveChangesAsync();

                db.Set<Account>().Any().Should().BeTrue();
                db.IsEntityInUse(db.Set<Account>().First()).Should().BeTrue();

                db.Set<User>().Remove(u);
                await db.SaveChangesAsync();

                db.Set<Account>().Any().Should().BeTrue();
                db.IsEntityInUse(db.Set<Account>().First()).Should().BeFalse();
            }
        }

        [TestMethod]
        public void TestUpdateFrom()
        {
            var user = new User();
            var user1 = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
                Account = new Account(),
            };

            user.UpdateFrom(user1);
            user.FirstName.Should().Be("Duy");
            user.LastName.Should().Be("Hoang");
            user.Account.Should().Be(user1.Account);
        }
    }
}
