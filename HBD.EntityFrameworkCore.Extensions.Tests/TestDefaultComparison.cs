using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
   [TestClass]
    public class TestDefaultComparison
    {
        [TestMethod]
        public void Test()
        {
            EqualityComparer<int>.Default.Equals(0, default(int)).Should().BeTrue();
            EqualityComparer<long>.Default.Equals(0, default(long)).Should().BeTrue();
        }
    }
}
