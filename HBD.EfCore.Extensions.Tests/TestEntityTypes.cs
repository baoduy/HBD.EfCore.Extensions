using FluentAssertions;
using HBD.EfCore.Extensions.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class TestEntityTypes
    {
        #region Public Methods

        [TestMethod]
        public void TestEntity_Audit()
        {
            typeof(Entity<int>).IsAssignableFrom(typeof(AuditEntity<int>))
                .Should().BeTrue();

            typeof(Entity<int>).IsAssignableFrom(typeof(AuditEntity))
                .Should().BeTrue();

            typeof(IEntity<int>).IsAssignableFrom(typeof(AuditEntity<int>))
                .Should().BeTrue();

            typeof(IEntity<int>).IsAssignableFrom(typeof(AuditEntity))
                .Should().BeTrue();
        }

        [TestMethod]
        public void TestEntity_AuditEntityGeneric()
        {
            typeof(IEntity<int>).IsAssignableFrom(typeof(IAuditEntity<int>))
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
            typeof(IEntity<int>).IsAssignableFrom(typeof(IAuditEntity))
                .Should().BeTrue();
        }

        [TestMethod]
        public void TestIAudit_Audit()
        {
            typeof(IAuditEntity<int>).IsAssignableFrom(typeof(AuditEntity))
                .Should().BeTrue();

            typeof(IAuditEntity).IsAssignableFrom(typeof(AuditEntity))
                .Should().BeTrue();
        }

        [TestMethod]
        public void TestIEntity_Entity()
        {
            typeof(IEntity<int>).IsAssignableFrom(typeof(Entity))
                .Should().BeTrue();

            typeof(IEntity).IsAssignableFrom(typeof(Entity))
                .Should().BeTrue();
        }

        [TestMethod]
        public void TestIEntity_IEntityGeneric()
        {
            typeof(IEntity<int>).IsAssignableFrom(typeof(IEntity))
                .Should().BeTrue();
        }

        #endregion Public Methods
    }
}