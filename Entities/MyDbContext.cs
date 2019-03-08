using Microsoft.EntityFrameworkCore;

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