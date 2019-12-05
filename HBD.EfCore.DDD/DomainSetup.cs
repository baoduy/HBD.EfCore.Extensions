using GenericEventRunner.ForSetup;
using HBD.Actions.Runner;
using HBD.EfCore.DDD.Internals;
using HBD.EfCore.DDD.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoMapper;

[assembly: InternalsVisibleTo("HBD.EfCore.DDD.Tests")]

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DomainSetup
    {
        #region Methods

        public static IServiceCollection AddBoundedContext<TContext>(this IServiceCollection service, Action<DbContextOptionsBuilder> contextBuilder,
                                    Assembly[] assembliesToScans, Expression<Func<Type, bool>> entityFilter = null, bool enableDebug = false) where TContext : DbContext =>
            service.AddDbContext<TContext>(
                op =>
                {
                    contextBuilder(op);
                    op.UseAutoConfigModel(o =>
                    {
                        var scan = o.ScanFrom(assembliesToScans);

                        if (entityFilter != null)
                            scan.WithFilter(entityFilter)
                            .IgnoreOthers();
                    });

                    if (enableDebug)
                        op.EnableDetailedErrors()
                          .EnableSensitiveDataLogging();
                }, ServiceLifetime.Scoped, ServiceLifetime.Singleton);

        public static IServiceCollection AddDomainServices(this IServiceCollection services, IGenericEventRunnerConfig eventConfig = null, params Assembly[] assembliesToScans)
        {
            services.AddRepositories()
                .AddDomainActionRunner()
                .AddAutoMapper(assembliesToScans);

            if (eventConfig == null)
                services.RegisterGenericEventRunner(assembliesToScans);
            else services.RegisterGenericEventRunner(eventConfig, assembliesToScans);

            return services;
        }

        public static IServiceCollection AddDomainWithSingleBoundedContext<TContext>(this IServiceCollection services,
            Action<DbContextOptionsBuilder> contextBuilder, IGenericEventRunnerConfig eventConfig = null,
            bool enableDebug = false, params Assembly[] assembliesToScans) where TContext : DbContext
            => services.AddDomainServices(eventConfig, assembliesToScans)
                .AddBoundedContext<TContext>(contextBuilder, assembliesToScans, null, enableDebug);

        public static IServiceCollection AsImplementedInterfaces(this IServiceCollection services, IEnumerable<Type> types, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (types == null) throw new ArgumentNullException(nameof(types));

            foreach (var classType in types)
            {
                if (classType.IsInterface) continue;

                var interfaces = classType.GetInterfaces()
                    .Where(i => i != typeof(IDisposable) && i.IsPublic);

                //Add Interfaces
                foreach (var infc in interfaces)
                    services.Add(new ServiceDescriptor(infc, classType, lifetime));

                //Add itself
                services.Add(new ServiceDescriptor(classType, classType, lifetime));
            }

            return services;
        }

        private static IServiceCollection AddDomainActionRunner(this IServiceCollection services)
            => services.AddActionRunner()
            .Replace(new ServiceDescriptor(typeof(IActionRunnerService), typeof(EventActionRunner), ServiceLifetime.Singleton));

        private static IServiceCollection AddRepositories(this IServiceCollection services)
            => services
                .AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>))
                .AddScoped(typeof(IDtoReadOnlyRepository<>), typeof(DtoReadOnlyRepository<>));

        #endregion Methods
    }
}