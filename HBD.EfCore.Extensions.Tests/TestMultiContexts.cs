using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repos.BoundedContexts;
using System.Runtime.InteropServices;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class TestMultiContexts
    {
        #region Properties

        public static string ConnectionString =>
           RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
           "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CodesERPDb;Integrated Security=True;Connect Timeout=300;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=True" :
           "Data Source=localhost;Initial Catalog=CodesERP;User Id=sa;Password=Pass@word1;";

        #endregion Properties

        #region Methods

        [TestMethod]
        public void ConfigMultiContexts()
        {
            var service = new ServiceCollection()
                .AddBoundedContexts(ConnectionString)
                .BuildServiceProvider();

            var account = service.GetService<AccountContext>();
            account.Database.EnsureCreated();
        }

        #endregion Methods
    }
}