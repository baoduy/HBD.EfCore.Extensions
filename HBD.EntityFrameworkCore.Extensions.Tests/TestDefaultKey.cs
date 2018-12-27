using DataLayer;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class TestDefaultKey
    {
        [TestMethod]
        public void DefaultKey()
        {
            new User().Id.Should().Be(0);
            new Address().Id.Should().Be(0);
        }
    }
}
