using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HBD.EfCore.Hooks.Triggers
{
    internal class TriggerHook : Hook
    {
        public override Task OnSaved(IReadOnlyCollection<EntityInfo> entities, DbContext dbContext, CancellationToken cancellationToken = default)
        {
            ExecuteAsync(entities, dbContext);
            return base.OnSaved(entities, dbContext, cancellationToken);
        }

        private static IReadOnlyCollection<ITriggerProfile> GetProfiles(DbContext dbContext,
            IServiceProvider serviceProvider)
        {
            var list = new List<ITriggerProfile>();

            if (dbContext != null)
                list.AddRange(dbContext.GetInfrastructure().GetServices<ITriggerProfile>());
            if (serviceProvider != null)
                list.AddRange(serviceProvider.GetServices<ITriggerProfile>());

            return list;
        }

        private static async void ExecuteAsync(IReadOnlyCollection<EntityInfo> entities, DbContext dbContext)
        {
            var groups = from e in entities
                         group e by e.EntityType into g
                         select new { EntityType = g.Key, Entries = g.ToList() };

            var context = dbContext.GetService<TriggerContext>();
            var profiles = GetProfiles(dbContext, context.ServiceProvider);

            if (!profiles.Any()) return;

            var tasks = from g in groups
                            // ReSharper disable once InconsistentlySynchronizedField
                        from r in profiles
                        where g.EntityType == r.EntityType
                              && r.TriggerType != TriggerType.None
                        select new { g.Entries, Profile = r };
            await Task.WhenAll(tasks.Select(e => ExecuteAsync(e.Entries, e.Profile, context)));
        }

        private static Task ExecuteAsync(IReadOnlyCollection<EntityInfo> entities, ITriggerProfile rule,
            TriggerContext context)
        {
            if (rule.TriggerType == TriggerType.All)
                return ((dynamic)rule).Execute(entities.Select(e => e.Entity as dynamic).ToList(), context);

            var final = new List<dynamic>();

            if (rule.TriggerType.HasFlag(TriggerType.Created))
                final.AddRange(entities.Where(e => e.State == EntityState.Added).Select(e => e.Entity));
            if (rule.TriggerType.HasFlag(TriggerType.Updated))
                final.AddRange(entities.Where(e => e.State == EntityState.Modified).Select(e => e.Entity));
            if (rule.TriggerType.HasFlag(TriggerType.Deleted))
                final.AddRange(entities.Where(e => e.State == EntityState.Deleted).Select(e => e.Entity));

            return ((dynamic)rule).Execute(final, context);
        }
    }
}
