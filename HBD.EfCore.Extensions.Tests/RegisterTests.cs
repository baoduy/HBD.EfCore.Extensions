using DataLayer;
using FluentAssertions;
using HBD.EfCore.Extensions.Abstractions;
using HBD.EfCore.Extensions.Configurations;
using HBD.EfCore.Extensions.Internal;
using HBD.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class RegisterTests
    {
        #region Methods

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

            await UnitTestSetup.Db.SaveChangesAsync().ConfigureAwait(false);

            Assert.IsTrue(await UnitTestSetup.Db.Set<User>().CountAsync().ConfigureAwait(false) >= 1);
            Assert.IsTrue(await UnitTestSetup.Db.Set<Address>().CountAsync().ConfigureAwait(false) >= 1);
        }

        [TestMethod]
        public async Task TestAccountStatusDataSeeding()
        {
            using var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()

                //No Assembly provided it will scan the MyDbContext assembly.
                .UseAutoConfigModel()
                .Options);
            await db.Database.EnsureCreatedAsync().ConfigureAwait(false);
            (await db.Set<AccountStatus>().CountAsync().ConfigureAwait(false)).Should().BeGreaterOrEqualTo(2);
        }

        [TestMethod]
        public async Task TestCreateDb()
        {
            using var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly)
                    .WithDefaultMappersType(typeof(EntityTypeConfiguration<>)))
                .Options);

            await db.Database.EnsureCreatedAsync().ConfigureAwait(false);

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

            await db.SaveChangesAsync().ConfigureAwait(false);

            var users = await db.Set<User>().ToListAsync().ConfigureAwait(false);
            var adds = await db.Set<Address>().ToListAsync().ConfigureAwait(false);

            Assert.IsTrue(users.Count == 1);
            Assert.IsTrue(adds.Count == 1);
        }

        [TestMethod]

        //[ExpectedException(typeof(DbUpdateException))]
        public async Task TestCreateDb_CustomMapper()
        {
            using var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()

                //No Assembly provided it will scan the MyDbContext assembly.
                .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly)
                    .WithDefaultMappersType(typeof(EntityTypeConfiguration<>)))
                .Options);
            await db.Database.EnsureCreatedAsync().ConfigureAwait(false);

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

            await db.SaveChangesAsync().ConfigureAwait(false);

            (await db.Set<Address>().AnyAsync().ConfigureAwait(false)).Should().BeTrue();
        }

        [TestMethod]
        public void TestCreateDb_NoAssembly()
        {
            using var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()

                //No Assembly provided it will scan the MyDbContext assembly.
                .UseAutoConfigModel(op => op.ScanFrom()
                    .WithDefaultMappersType(typeof(EntityTypeConfiguration<>)))
                .Options);
        }

        [TestMethod]

        //[ExpectedException(typeof(DbUpdateException))]
        public async Task TestCreateDb_Validate()
        {
            using var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly)
                    .WithDefaultMappersType(typeof(EntityTypeConfiguration<>)))
                .Options);
            await db.Database.EnsureCreatedAsync().ConfigureAwait(false);

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

            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        [TestMethod]
        public async Task TestEnumStatus1DataSeeding()
        {
            using var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()

                //No Assembly provided it will scan the MyDbContext assembly.
                .UseAutoConfigModel()
                .Options);
            await db.Database.EnsureCreatedAsync().ConfigureAwait(false);
            (await db.Set<EnumTables<EnumStatus1>>().CountAsync().ConfigureAwait(false)).Should().Be(3);
        }

        [TestMethod]
        public async Task TestEnumStatusDataSeeding()
        {
            using var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()

                //No Assembly provided it will scan the MyDbContext assembly.
                .UseAutoConfigModel()
                .Options);
            await db.Database.EnsureCreatedAsync().ConfigureAwait(false);
            (await db.Set<EnumTables<EnumStatus>>().CountAsync().ConfigureAwait(false)).Should().Be(3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestIgnoredEntityAsync()
        {
            using var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .UseAutoConfigModel(op =>
                    op.ScanFrom(typeof(MyDbContext).Assembly)
                      .WithDefaultMappersType(typeof(EntityTypeConfiguration<>)))
                .Options);

            db.Database.EnsureCreated();

            var list = await db.Set<IgnoredAutoMapperEntity>().ToListAsync();
        }

        [TestMethod]

        //[Ignore]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWithCustomEntityMapper_Bad()
        {
            using var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .UseAutoConfigModel(op =>
                    op.ScanFrom(typeof(MyDbContext).Assembly).WithDefaultMappersType(typeof(Entity<>)))
                .Options);

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWithCustomEntityMapper_NullFilter_Bad()
        {
            using var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .UseAutoConfigModel(op =>
                    op.ScanFrom(typeof(MyDbContext).Assembly).WithFilter(null))
                .Options);
        }

        #endregion Methods
    }
}