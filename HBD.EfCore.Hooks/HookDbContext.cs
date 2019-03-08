using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks
{
    public abstract class HookDbContext : DbContext
    {
        #region Protected Constructors

        protected HookDbContext()
        {
        }

        protected HookDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// Allow to disable OnSaving event. If this is True the OnSaving of <see cref="ISavingAware"/> won't be executed.
        /// </summary>
        public bool DisableSavingAwareness { get; set; }

        /// <summary>
        /// Run Deep validation before Save
        /// </summary>
        public bool DeepValidation { get; set; } = true;

        #endregion Public Properties

        #region Protected Methods

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
            => this.SaveChangesWithHooks(base.SaveChanges, acceptAllChangesOnSuccess);

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
            => this.SaveChangesWithHooksAsync(base.SaveChangesAsync, acceptAllChangesOnSuccess, cancellationToken);

        #endregion Protected Methods
    }
}