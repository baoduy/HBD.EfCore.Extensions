using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks
{
    public abstract class Hook : IHook
    {
        public virtual Task OnSaving(IReadOnlyCollection<EntityInfo> entities, DbContext dbContext, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public virtual Task OnSaved(IReadOnlyCollection<EntityInfo> entities, DbContext dbContext, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}
