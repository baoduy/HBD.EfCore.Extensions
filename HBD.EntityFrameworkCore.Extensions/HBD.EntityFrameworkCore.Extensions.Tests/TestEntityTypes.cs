using FluentAssertions;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class TestEntityTypes
    {
        [TestMethod]
        public void TestIEntity_IEntityGeneric()
        {
            typeof(IEntity<long>).IsAssignableFrom(typeof(IEntity))
                .Should().BeTrue();
        }

        [TestMethod]
        public void TestIEntity_Entity()
        {
            typeof(IEntity<long>).IsAssignableFrom(typeof(Entity))
                .Should().BeTrue();

            typeof(IEntity).IsAssignableFrom(typeof(Entity))
                .Should().BeTrue();
        }

        [TestMethod]
        public void TestEntity_AuditEntityGeneric()
        {
            typeof(IEntity<long>).IsAssignableFrom(typeof(IAuditEntity<long>))
                .Should().BeTrue();
        }

        [TestMethod]
        public void TestEntity_AuditGeneric()
        {
            typeof(IEntity).IsAssignableFrom(typeof(IAuditEntity))
                .Should().BeTrue();
        }

        [TestMethod]
        public void TestEntityGeneric_AuditGeneric()
        {
            typeof(IEntity<long>).IsAssignableFrom(typeof(IAuditEntity))
                .Should().BeTrue();
        }

        [TestMethod]
        public void TestIAudit_Audit()
        {
            typeof(IAuditEntity<long>).IsAssignableFrom(typeof(AuditEntity))
                .Should().BeTrue();

            typeof(IAuditEntity).IsAssignableFrom(typeof(AuditEntity))
                .Should().BeTrue();
        }

        [TestMethod]
        public void TestEntity_Audit()
        {
            typeof(Entity<long>).IsAssignableFrom(typeof(AuditEntity<long>))
                .Should().BeTrue();

            typeof(Entity<long>).IsAssignableFrom(typeof(AuditEntity))
                .Should().BeTrue();

            typeof(IEntity<long>).IsAssignableFrom(typeof(AuditEntity<long>))
                .Should().BeTrue();

            typeof(IEntity<long>).IsAssignableFrom(typeof(AuditEntity))
                .Should().BeTrue();
        }
    }
}
