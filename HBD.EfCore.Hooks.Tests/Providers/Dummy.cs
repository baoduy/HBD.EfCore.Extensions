using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HBD.EfCore.Hooks.Triggers;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks.Tests.Providers
{
    internal class Dummy : IHook
    {
        public int OnSavingCalled { get; private set; }
        public int OnSavedCalled { get; private set; }

        public Task OnSaving(IReadOnlyCollection<TriggerEntityState> entities, DbContext dbContext,
             CancellationToken cancellationToken = default)
        {
            OnSavingCalled += 1;
            return Task.CompletedTask;
        }

        public Task OnSaved(IReadOnlyCollection<TriggerEntityState> entities, DbContext dbContext,
            CancellationToken cancellationToken = default)
        {
            OnSavedCalled += 1;
            return Task.CompletedTask;
        }
    }
}
