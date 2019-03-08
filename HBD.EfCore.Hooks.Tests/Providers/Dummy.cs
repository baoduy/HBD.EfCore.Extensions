using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks.Tests.Providers
{
    internal class Dummy : IHook
    {
        public bool OnSavingCalled { get; private set; }
        public bool OnSavedCalled { get; private set; }

        public Task OnSaving(IReadOnlyCollection<EntityInfo> entities, DbContext dbContext,
             CancellationToken cancellationToken = default)
        {
            OnSavingCalled = true;
            return Task.CompletedTask;
        }

        public Task OnSaved(IReadOnlyCollection<EntityInfo> entities, DbContext dbContext,
            CancellationToken cancellationToken = default)
        {
            OnSavedCalled = true;
            return Task.CompletedTask;
        }
    }
}
