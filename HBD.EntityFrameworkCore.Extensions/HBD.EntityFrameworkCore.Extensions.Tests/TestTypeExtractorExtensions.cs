﻿using System.Linq;
using DataLayer;
using FluentAssertions;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class TestTypeExtractorExtensions
    {
        [TestMethod]
        public void TestExtract()
        {
            typeof(MyDbContext).Assembly.Extract().Public().Class().Count()
                .Should().BeGreaterOrEqualTo(3);
        }

        [TestMethod]
        public void TestExtract_NotInstanceOf()
        {
            var list = typeof(MyDbContext).Assembly.Extract().Class().NotInstanceOf(typeof(IEntity<long>)).ToList();
            list.Contains(typeof(User)).Should().BeFalse();
            list.Contains(typeof(Address)).Should().BeFalse();
        }

        [TestMethod]
        public void TestScanPublicClassesFromWithFilter()
        {
            typeof(MyDbContext).Assembly.ScanPublicClassesFromWithFilter("Context")
                .Count().Should().Be(1);
        }

        [TestMethod]
        public void TestScanClassesFromWithFilter()
        {
            typeof(MyDbContext).Assembly.ScanClassesFromWithFilter("Mapper")
                .Count().Should().BeGreaterOrEqualTo(1);
        }


        [TestMethod]
        public void TestScanPublicClassesImplementOf()
        {
            typeof(MyDbContext).Assembly.ScanPublicClassesImplementOf<AuditEntity>()
                .Count().Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        public void TestScanClassesImplementOf()
        {
            typeof(MyDbContext).Assembly.ScanClassesImplementOf(typeof(Entity))
                .Count().Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        public void TestScanClassesImplementOf_Generic()
        {
            typeof(MyDbContext).Assembly.ScanClassesImplementOf<IEntity<long>>()
                .Count().Should().BeGreaterOrEqualTo(2);
        }
    }
}
