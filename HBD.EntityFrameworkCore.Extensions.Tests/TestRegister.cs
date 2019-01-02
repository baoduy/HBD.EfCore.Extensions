using DataLayer;
using DataLayer.Mappers;
using FluentAssertions;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using HBD.EntityFrameworkCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class TestRegister
    {
        #region Public Methods

        [TestMethod]
        public async Task Test_RegisterEntities_DefaultOptions()
        {
            //Create User with Address
            await UnitTestSetup.Db.Set<User>().AddAsync(new User("Duy")
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

            await UnitTestSetup.Db.SaveChangesAsync();

            Assert.IsTrue(await UnitTestSetup.Db.Set<User>().CountAsync() >= 1);
            Assert.IsTrue(await UnitTestSetup.Db.Set<Address>().CountAsync() >= 1);
        }

        [TestMethod]
        public async Task TestAccountStatusDataSeeding()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                //No Assembly provided it will scan the MyDbContext assembly.
                .RegisterEntities()
                .Options))
            {
                await db.Database.EnsureCreatedAsync();
                (await db.Set<AccountStatus>().CountAsync()).Should().BeGreaterOrEqualTo(2);
            }
        }

        [TestMethod]
        public async Task TestCreateDb()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
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
                var adds = await db.Set<Address>().ToListAsync();

                Assert.IsTrue(users.Count == 1);
                Assert.IsTrue(adds.Count == 1);
            }
        }

        [TestMethod]
        //[ExpectedException(typeof(DbUpdateException))]
        public async Task TestCreateDb_CustomMapper()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                //No Assembly provided it will scan the MyDbContext assembly.
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly)
                    .WithDefaultMappersType(typeof(AuditEntityMapper<>)))
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
                        new Address {Street = "123"}
                    }
                });

                await db.SaveChangesAsync();

                (await db.Set<Address>().AnyAsync()).Should().BeTrue();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateDb_NoAssembly()
        {
            var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                //No Assembly provided it will scan the MyDbContext assembly.
                .RegisterEntities(op => op.FromAssemblies()
                    .WithDefaultMappersType(typeof(AuditEntityMapper<>)))
                .Options);
        }

        [TestMethod]
        //[ExpectedException(typeof(DbUpdateException))]
        public async Task TestCreateDb_Validate()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
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
                        new Address {Street = "123"}
                    }
                });

                await db.SaveChangesAsync();
            }
        }

        [TestMethod]
        public async Task TestEnumStatusDataSeeding()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                //No Assembly provided it will scan the MyDbContext assembly.
                .RegisterEntities()
                .Options))
            {
                await db.Database.EnsureCreatedAsync();
                (await db.Set<EnumTables<EnumStatus>>().CountAsync()).Should().Be(3);
            }
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWithCustomEntityMapper_Bad()
        {
            var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .RegisterEntities(op =>
                    op.FromAssemblies(typeof(MyDbContext).Assembly).WithDefaultMappersType(typeof(Entity<>)))
                .Options);

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWithCustomEntityMapper_NullFilter_Bad()
        {
            var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .RegisterEntities(op =>
                    op.FromAssemblies(typeof(MyDbContext).Assembly).WithFilter(null))
                .Options);
        }

        #endregion Public Methods
    }
}