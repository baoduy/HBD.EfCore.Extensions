using System;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit.Extensions.AssertExtensions;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        private async Task AddData(DbContext dbContext)
        {
            await dbContext.Database.EnsureCreatedAsync();

            //Create User with Address
            await dbContext.Set<User>().AddAsync(new User("Duy")
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

        //[TestMethod]
        //public async Task Address_Is_InUse()
        //{
        //    using (var db = new MyDbContext(new DbContextOptionsBuilder()
        //        .UseSqliteMemory()
        //        .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
        //        .Options))
        //    {
        //        db.LogToConsole();
        //        await AddData(db);

        //        var address = db.Set<Address>().First();
        //        //Referencing by User
        //        db.IsEntityReferenceByOthers(address).Should().BeTrue();
        //    }
        //}

        //[TestMethod]
        //public async Task User_IsNot_InUse()
        //{
        //    using (var db = new MyDbContext(new DbContextOptionsBuilder()
        //        .UseSqliteMemory()
        //        .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
        //        .Options))
        //    {
        //        db.LogToConsole();
        //        await AddData(db);

        //        var u = db.Set<User>().First();
        //        //User is not referencing by Address
        //        db.IsEntityReferenceByOthers(u).Should().BeFalse();
        //    }
        //}

        //[TestMethod]
        //public async Task User_IsNot_InUse_WithOneToOne()
        //{
        //    using (var db = new MyDbContext(new DbContextOptionsBuilder()
        //        .UseSqliteMemory()
        //        .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
        //        .Options))
        //    {
        //        db.LogToConsole();
        //        await AddData(db);

        //        var u = db.Set<User>().First();
        //        var account = new Account { UserName = "1", Password = "1" };
        //        u.Account = account;
        //        account.User = u;

        //        await db.SaveChangesAsync();

        //        db.Set<Account>().Any().Should().BeTrue();
        //        db.IsEntityReferenceByOthers(db.Set<Account>().First()).Should().BeTrue();

        //        db.Set<User>().Remove(u);
        //        await db.SaveChangesAsync();

        //        db.Set<Account>().Any().Should().BeTrue();
        //        db.IsEntityReferenceByOthers(db.Set<Account>().First()).Should().BeFalse();
        //    }
        //}

        [TestMethod]
        public void TestUpdateFrom()
        {
            var user = new User("Duy");
            var user1 = new User("Steven")
            {
                FirstName = "Duy",
                LastName = "Hoang",
                Account = new Account(),
            };

            user1.UpdatedByUser("Hoang");

            user.UpdateFrom(user1);
            user.FirstName.Should().Be("Duy");
            user.LastName.Should().Be("Hoang");
            user.Account.Should().Be(user1.Account);
            user.CreatedBy.Should().Be("Duy");
            user.UpdatedBy.Should().BeNullOrEmpty();
        }

        [TestMethod]
        public void TestUpdateFrom_NotIgnoreNull()
        {
            var user = new User("Duy") { Account = new Account() };
            var user1 = new User("Duy")
            {
                FirstName = "Duy",
                LastName = "Hoang",
                Account = null
            };

            user.UpdateFrom(user1, ignoreNull: false);
            user.FirstName.Should().Be("Duy");
            user.LastName.Should().Be("Hoang");
            user.Account.Should().BeNull();
        }

        [TestMethod]
        public void TestUpdateFrom_IgnoreNull()
        {
            var user = new User("Duy") { Account = new Account() };
            var user1 = new User("Duy")
            {
                FirstName = "Duy",
                LastName = "Hoang",
                Account = null
            };

            user.UpdateFrom(user1, ignoreNull: true);
            user.FirstName.Should().Be("Duy");
            user.LastName.Should().Be("Hoang");
            user.Account.Should().NotBeNull();
        }
    }
}
