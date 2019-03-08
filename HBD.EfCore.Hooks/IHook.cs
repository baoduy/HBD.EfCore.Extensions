using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks
{
    public interface IHook
    {
        Task OnSaving(IReadOnlyCollection<EntityInfo> entities, DbContext dbContext, CancellationToken cancellationToken = default);
        Task OnSaved(IReadOnlyCollection<EntityInfo> entities, DbContext dbContext, CancellationToken cancellationToken = default);
    }
}
