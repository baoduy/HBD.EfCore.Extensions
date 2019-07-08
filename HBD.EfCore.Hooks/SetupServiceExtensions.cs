using System;
using HBD.EfCore.Hooks;
using HBD.EfCore.Hooks.DeepValidation;
using HBD.EfCore.Hooks.Internal;
using HBD.EfCore.Hooks.Options;
using HBD.EfCore.Hooks.SavingAwareness;
using HBD.EfCore.Hooks.Triggers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class SetupServiceExtensions
    {
        #region Public Methods

        /// <summary>
        /// Register a DbContext to the Pool with Hook
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="dbContextOptions"></param>
        /// <param name="hookOptions"></param>
        /// <param name="poolSize"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbContextPoolWithHooks<TContext>(
            this IServiceCollection serviceCollection,
            Action<SetupOptions> hookOptions,
            Action<DbContextOptionsBuilder> dbContextOptions = null,
            int poolSize = 128)
            where TContext : DbContext =>
            serviceCollection.AddDbContextPool<TContext>(builder =>
            {
                dbContextOptions?.Invoke(builder);
                builder.BuildDbContextWithHooksOptions(serviceCollection, hookOptions);
            }, poolSize);


        /// <summary>
        /// Register a DbContext with Hook
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="dbContextOptions"></param>
        /// <param name="hookOptions"></param>
        /// <param name="contextLifetime"></param>
        /// <param name="optionsLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbContextWithHooks<TContext>(
            this IServiceCollection serviceCollection,
            Action<SetupOptions> hookOptions,
            Action<DbContextOptionsBuilder> dbContextOptions = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TContext : DbContext =>
            serviceCollection.AddDbContext<TContext>(builder =>
           {
               dbContextOptions?.Invoke(builder);
               builder.BuildDbContextWithHooksOptions(serviceCollection, hookOptions);
           }, contextLifetime, optionsLifetime);

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        ///     Use SavingAwareness, Deep validation and Trigger Hooks
        /// </summary>
        /// <param name="this"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        internal static DbContextOptionsBuilder AddServices(
            this DbContextOptionsBuilder @this,
            Action<IServiceCollection> factory)
        {
            var sv = GetServiceInjectionExtension(@this);
            sv.Includes(factory);
            return @this;
        }

        /// <summary>
        ///     Use Custom hooks. Please note that this will not add SavingAwareness, Deep validation and
        ///     Trigger Hooks by default
        /// </summary>
        /// <param name="this"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private static DbContextOptionsBuilder UseHooks(this DbContextOptionsBuilder @this,
            Action<HookOptions> options)
        {
            var op = new HookOptions();
            options.Invoke(op);

            @this.AddServices(s =>
            {
                var i = typeof(IHook);
                foreach (var hook in op.HooksInstance)
                    s.AddSingleton(i, hook);
                foreach (var t in op.HooksTypes)
                    s.AddSingleton(i, t);
            });

            return @this;
        }

        #endregion Internal Methods

        #region Private Methods

        private static SetupOptions BuildDbContextWithHooksOptions(
            this DbContextOptionsBuilder builder,
            IServiceCollection globalServiceCollection,
            Action<SetupOptions> hookOptions)
        {
            var setupOptions = new SetupOptions();
            hookOptions.Invoke(setupOptions);

            builder.AddServices(op => op.AddSingleton(setupOptions));

            if (setupOptions.Assemblies == null)
                setupOptions.ScanFrom(builder.Options.ContextType.Assembly);

            if (setupOptions.DeepValidation)
                builder.UseHooks(op => op.Add<DeepValidationHook>());

            if (setupOptions.SavingAwareness)
                builder.UseHooks(op => op.Add<SavingAwarenessHook>());

            if (setupOptions.Trigger)
            {
                //builder.AddServices(sv => sv.UseTriggerProfiles(setupOptions.Assemblies));
                globalServiceCollection.UseTriggerProfiles(setupOptions.Assemblies);
                builder.UseHooks(op => op.Add<TriggerHook>());
            }

            builder.UseHooks(op => op.ScanFrom(setupOptions.Assemblies));
            return setupOptions;
        }

        private static IServiceInjectionExtension GetServiceInjectionExtension(
            this DbContextOptionsBuilder optionsBuilder)
        {
            var op = optionsBuilder.Options.FindExtension<ServiceInjectionExtension>();

            if (op == null)
            {
                op = new ServiceInjectionExtension();
                ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(op);
            }

            return op;
        }

        #endregion Private Methods
    }
}