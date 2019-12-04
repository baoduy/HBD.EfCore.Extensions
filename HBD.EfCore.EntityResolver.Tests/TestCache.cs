using FluentAssertions;
using HBD.EfCore.EntityResolvers.Internal;
using HBD.EfCore.EntityResolver.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.EntityResolver.Tests
{
    [TestClass]
    public class TestCache
    {
        #region Methods

        [TestMethod]
        public void TestPropertyInfoCache()
        {
            var p = InternalCache.GetModelInfo(new UserModel());
            var p2 = InternalCache.GetModelInfo(new UserModel());

            p.Should().BeSameAs(p2);
        }

        #endregion Methods
    }
}