using System;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using DataLayer.Mappers;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using TestSupport.EfHelpers;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class TestRegister
    {
        [TestMethod]
        public async Task TestCreateDb()
        {
            var options = SqliteInMemory.CreateOptions<MyDbContext>();

            using (var db = new MyDbContext(new DbContextOptionsBuilder(options)
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
                .Options))
            {
                await db.Database.EnsureCreatedAsync();

                //Create User with Address
                await db.Set<User>().AddAsync(new User
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
                var adds = await db.Set<Address>().ToListAsync();

                Assert.IsTrue(users.Count == 1);
                Assert.IsTrue(adds.Count == 1);
            }
        }

        [TestMethod]
        public async Task Test_RegisterEntities_DefaultOptions()
        {
            var options = SqliteInMemory.CreateOptions<MyDbContext>();

            using (var db = new MyDbContext(new DbContextOptionsBuilder(options)
                //No Assembly provided it will scan the MyDbContext assembly.
                .RegisterEntities()
                .Options))
            {
                await db.Database.EnsureCreatedAsync();

                //Create User with Address
                await db.Set<User>().AddAsync(new User
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

                Assert.IsTrue(await db.Set<User>().CountAsync() == 1);
                Assert.IsTrue(await db.Set<Address>().CountAsync() == 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task TestCreateDb_Validate()
        {
            var options = SqliteInMemory.CreateOptions<MyDbContext>();

            using (var db = new MyDbContext(new DbContextOptionsBuilder(options)
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
                .Options))
            {
                await db.Database.EnsureCreatedAsync();

                //Create User with Address
                await db.Set<User>().AddAsync(new User
                {
                    FirstName = "Duy",
                    LastName = "Hoang",
                    Addresses = {
                        new Address()
                    }
                });

                await db.SaveChangesAsync();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task TestCreateDb_CustomMapper()
        {
            var options = SqliteInMemory.CreateOptions<MyDbContext>();

            using (var db = new MyDbContext(new DbContextOptionsBuilder(options)
                //No Assembly provided it will scan the MyDbContext assembly.
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly)
                    .WithDefaultMapperType(typeof(CustomEntityMapper<>)))
                .Options))
            {
                await db.Database.EnsureCreatedAsync();

                //Create User with Address
                await db.Set<User>().AddAsync(new User
                {
                    FirstName = "Duy",
                    LastName = "Hoang",
                    Addresses = {
                        new Address()
                    }
                });

                await db.SaveChangesAsync();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateDb_NoAssembly()
        {
            var options = SqliteInMemory.CreateOptions<MyDbContext>();

            var db = new MyDbContext(new DbContextOptionsBuilder(options)
                //No Assembly provided it will scan the MyDbContext assembly.
                .RegisterEntities(op => op.FromAssemblies()
                    .WithDefaultMapperType(typeof(CustomEntityMapper<>)))
                .Options);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWithCustomEntityMapper_Bad()
        {
            var options = SqliteInMemory.CreateOptions<MyDbContext>();

            var db = new MyDbContext(new DbContextOptionsBuilder(options)
                .RegisterEntities(op =>
                    op.FromAssemblies(typeof(MyDbContext).Assembly).WithDefaultMapperType(typeof(Entity<>)))
                .Options);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWithCustomEntityMapper_NullFilter_Bad()
        {
            var options = SqliteInMemory.CreateOptions<MyDbContext>();

            var db = new MyDbContext(new DbContextOptionsBuilder(options)
                .RegisterEntities(op =>
                    op.FromAssemblies(typeof(MyDbContext).Assembly).WithFilter(null))
                .Options);
        }
    }
}
