using HBD.EfCore.Hooks.DeepValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HBD.EfCore.Hooks
{
    public static class Extensions
    {
        #region Internal Methods

        /// <summary>
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="saveChanges"></param>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        internal static int SaveChangesWithHooks(this DbContext dbContext,
            Func<bool, int> saveChanges,
            bool acceptAllChangesOnSuccess = true)
        {
            //if (dbContext is HookDbContext)
            //    throw new InvalidOperationException($"The {dbContext.GetType().Namespace} is instance of {nameof(HookDbContext)}. Please call {nameof(dbContext.SaveChanges)} instead.");

            var entries = dbContext.GetEntities();

            dbContext.OnSaving(entries).GetAwaiter().GetResult();
            var result = saveChanges(acceptAllChangesOnSuccess);
            dbContext.OnSaved(entries).GetAwaiter().GetResult();
            return result;
        }

        internal static async Task<int> SaveChangesWithHooksAsync(this DbContext dbContext,
            Func<bool, CancellationToken, Task<int>> saveChangesAsync,
            bool acceptAllChangesOnSuccess = true,
            CancellationToken cancellationToken = default)
        {
            //if (dbContext is HookDbContext)
            //    throw new InvalidOperationException($"The {dbContext.GetType().Namespace} is instance of {nameof(HookDbContext)}. Please call {nameof(dbContext.SaveChanges)} instead.");

            var entries = dbContext.GetEntities();

            await dbContext.OnSaving(entries, cancellationToken);
            var result = await saveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await dbContext.OnSaved(entries, cancellationToken);
            return result;
        }

        #endregion Internal Methods

        #region Private Methods

        private static IReadOnlyCollection<IHook> EnsureValidationHookAtLast(this IEnumerable<IHook> hooks)
        {
            var list = new List<IHook>(hooks);
            var i = 0;
            var j = list.Count;

            while (i < j)
            {
                if (list[i] is DeepValidationHook)
                {
                    var h = list[i];
                    list.RemoveAt(i);
                    list.Add(h);
                    j -= 1;
                }

                i++;
            }

            return list;
        }

        /// <summary>
        /// These will get all Entity from track changes without any filtering. The Hook need to filter the entity based on state itself
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        private static IReadOnlyCollection<EntityInfo> GetEntities(this DbContext dbContext) =>
            dbContext.ChangeTracker.Entries().Select(e => new EntityInfo(e.Entity, e.Metadata.ClrType, e.State)).ToList();

        private static IReadOnlyCollection<IHook> GetHooks(this DbContext dbContext)
            => dbContext.GetInfrastructure().GetServices<IHook>().EnsureValidationHookAtLast();

        private static Task OnSaved(this DbContext dbContext, IReadOnlyCollection<EntityInfo> entities,
                            CancellationToken cancellationToken = default)
        {
            var services = dbContext.GetHooks();
            return services.Any()
                 ? Task.WhenAll(services.Select(p => p.OnSaved(entities, dbContext, cancellationToken)))
                 : Task.CompletedTask;
        }

        private static Task OnSaving(this DbContext dbContext, IReadOnlyCollection<EntityInfo> entities,
            CancellationToken cancellationToken = default)
        {
            var services = dbContext.GetHooks();
            return services.Any()
                ? Task.WhenAll(services.Select(p => p.OnSaving(entities, dbContext, cancellationToken)))
                : Task.CompletedTask;
        }

        #endregion Private Methods
    }
}