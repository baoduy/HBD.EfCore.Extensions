using Microsoft.EntityFrameworkCore;

namespace Repos.BoundedContexts
{
    internal abstract class DbContextBase : DbContext
    {
        #region Constructors

        protected DbContextBase(DbContextOptions options) : base(options)
        {
        }

        #endregion Constructors
    }
}