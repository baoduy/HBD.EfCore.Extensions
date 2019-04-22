using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HBD.EfCore.Hooks.Triggers;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks
{
    public interface IHook
    {
        Task OnSaving(IReadOnlyCollection<TriggerEntityState> entities, DbContext dbContext, CancellationToken cancellationToken = default);
        Task OnSaved(IReadOnlyCollection<TriggerEntityState> entities, DbContext dbContext, CancellationToken cancellationToken = default);
    }
}
