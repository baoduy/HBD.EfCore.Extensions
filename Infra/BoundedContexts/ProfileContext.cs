using Domains.Profiles;
using Microsoft.EntityFrameworkCore;

namespace Repos.BoundedContexts
{
    internal class ProfileContext : DbContextBase
    {
        #region Constructors

        public ProfileContext(DbContextOptions<ProfileContext> options) : base(options)
        {
        }

        #endregion Constructors

        #region Properties

        public virtual DbSet<Profile> Profiles { get; private set; }

        #endregion Properties
    }
}