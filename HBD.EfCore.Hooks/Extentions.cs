using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HBD.EfCore.Hooks.DeepValidation;
using HBD.EfCore.Hooks.Triggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HBD.EfCore.Hooks
{
    public static class HookExtensions
    {
        #region Methods

        /// <summary>
        ///     Get changed properties includes navigation properties.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetChangedProperties(this EntityEntry @this)
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            foreach (var name in @this.Properties.Where(p => p.IsModified).Select(p => p.Metadata.Name))
                yield return name;

            foreach (var name in @this.Navigations.Where(p => p.IsModified).Select(p => p.Metadata.Name))
                yield return name;
        }

        /// <summary>
        ///     Action SaveChangesWithHooks
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

            var entries = dbContext.GetChangedEntities();

            dbContext.OnSaving(entries).GetAwaiter().GetResult();
            var result = saveChanges(acceptAllChangesOnSuccess);
            dbContext.OnSaved(entries).GetAwaiter().GetResult();
            return result;
        }

        /// <summary>
        ///     Action SaveChangesWithHooksAsync
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="saveChangesAsync"></param>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        internal static async Task<int> SaveChangesWithHooksAsync(this DbContext dbContext,
            Func<bool, CancellationToken, Task<int>> saveChangesAsync,
            bool acceptAllChangesOnSuccess = true,
            CancellationToken cancellationToken = default)
        {
            var entries = dbContext.GetChangedEntities();

            await dbContext.OnSaving(entries, cancellationToken).ConfigureAwait(false);
            var result = await saveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(false);
            await dbContext.OnSaved(entries, cancellationToken).ConfigureAwait(false);
            return result;
        }

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
        ///     These will get all Entity from track changes without any filtering. The Hook need to
        ///     filter the entity based on state itself
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        private static IReadOnlyCollection<TriggerEntityState> GetChangedEntities(this DbContext dbContext) =>
            dbContext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added
                            || e.State == EntityState.Modified
                            || e.State == EntityState.Deleted)
                .Select(e =>
            {
                var props = e.GetChangedProperties();
                return new TriggerEntityState(e.Entity, props, e.Metadata.ClrType, e.State);
            }).ToList();

        /// <summary>
        ///     Get all Hooks instances from ServiceProvider.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        private static IReadOnlyCollection<IHook> GetHooks(this DbContext dbContext)
            => dbContext.GetInfrastructure().GetServices<IHook>().EnsureValidationHookAtLast();

        /// <summary>
        ///     OnSaved event
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static Task OnSaved(this DbContext dbContext, IReadOnlyCollection<TriggerEntityState> entities,
            CancellationToken cancellationToken = default)
        {
            var hooks = dbContext.GetHooks();
            return hooks.Any()
                ? Task.WhenAll(hooks.Select(p => p.OnSaved(entities, dbContext, cancellationToken)))
                : Task.CompletedTask;
        }

        /// <summary>
        ///     OnSaving event
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static Task OnSaving(this DbContext dbContext, IReadOnlyCollection<TriggerEntityState> entities,
            CancellationToken cancellationToken = default)
        {
            var hooks = dbContext.GetHooks();
            return hooks.Any()
                ? Task.WhenAll(hooks.Select(p => p.OnSaving(entities, dbContext, cancellationToken)))
                : Task.CompletedTask;
        }

        #endregion Methods
    }
}