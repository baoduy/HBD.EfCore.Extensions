using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks.SavingAwareness
{
    public class SavingAwarenessHook : Hook
    {
        #region Public Methods

        public override Task OnSaving(IReadOnlyCollection<EntityInfo> entities, DbContext dbContext,
                    CancellationToken cancellationToken = default)
        {
            if (!dbContext.ChangeTracker.HasChanges()) return Task.CompletedTask;

            var tasks = from e in entities
                                    where e.Entity is ISavingAware
                                    select ((ISavingAware)e.Entity).OnSavingAsync(e.State, dbContext);

            cancellationToken.ThrowIfCancellationRequested();

            return Task.WhenAll(tasks);
        }

        #endregion Public Methods
    }
}