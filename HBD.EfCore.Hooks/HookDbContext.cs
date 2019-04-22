using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks
{
    public abstract class HookDbContext : DbContext
    {
        #region Protected Constructors

        /// <summary>
        /// Disable all Hooks
        /// </summary>
        public bool DisableHook { get; set; }


        protected HookDbContext()
        {
        }

        protected HookDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        public override int SaveChanges(bool acceptAllChangesOnSuccess) =>
            DisableHook
                ? base.SaveChanges(acceptAllChangesOnSuccess)
                : this.SaveChangesWithHooks(base.SaveChanges, acceptAllChangesOnSuccess);

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default) =>
            DisableHook
                ? base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken)
                : this.SaveChangesWithHooksAsync(base.SaveChangesAsync, acceptAllChangesOnSuccess, cancellationToken);

        #endregion Public Methods
    }
}