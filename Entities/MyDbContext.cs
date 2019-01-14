using Microsoft.EntityFrameworkCore;
using DbContext = HBD.EfCore.Extensions.DbContext;

namespace DataLayer
{
    public class MyDbContext : DbContext
    {
        #region Public Constructors

        public MyDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion Public Constructors
    }
}