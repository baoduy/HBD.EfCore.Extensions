using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HBD.EfCore.Hooks.Triggers;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks.SavingAwareness
{
    public sealed class SavingAwarenessHook : Hook
    {
        #region Properties

        /// <summary>
        /// Disable SavingAwarenessHook
        /// </summary>
        public static bool Disabled { get; set; }

        #endregion Properties

        #region Methods

        public override Task OnSaving(IReadOnlyCollection<TriggerEntityState> entities, DbContext dbContext,
                    CancellationToken cancellationToken = default)
        {
            if (dbContext is null)
            {
                throw new System.ArgumentNullException(nameof(dbContext));
            }

            if (Disabled) return Task.CompletedTask;

            if (!dbContext.ChangeTracker.HasChanges()) return Task.CompletedTask;

            var tasks = from e in entities
                        where e.Entity is ISavingAware
                        select ((ISavingAware)e.Entity).OnSavingAsync(e.State, dbContext);

            cancellationToken.ThrowIfCancellationRequested();

            return Task.WhenAll(tasks);
        }

        #endregion Methods
    }
}