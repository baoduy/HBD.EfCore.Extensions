using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Extensions.Tests
{
   [TestClass]
    public class DefaultComparisonTests
    {
        [TestMethod]
        public void Test()
        {
            EqualityComparer<int>.Default.Equals(0, default).Should().BeTrue();
            EqualityComparer<long>.Default.Equals(0, default).Should().BeTrue();
        }
    }
}
