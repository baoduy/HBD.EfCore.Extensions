using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class BenchmarkTest
    {
        #region Public Methods

        [Benchmark]
        public async Task TestCreateDb_CustomMapper()
        {
            using (var db = new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                //No Assembly provided it will scan the MyDbContext assembly.
                .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
                .Options))
            {
                await db.Database.EnsureCreatedAsync();
            }
        }

        [TestMethod]
        public void TestRegister()
        {
            var summary = BenchmarkRunner.Run<BenchmarkTest>();
        }

        
        [TestMethod]
        public void TestSpecs()
        {
            var summary = BenchmarkRunner.Run<TestSpec>();
        }

        #endregion Public Methods
    }
}