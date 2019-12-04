using Domains;
using Domains.Abstracts;
using HBD.Framework.Extensions;
using Microsoft.EntityFrameworkCore;
using Repos.BoundedContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HBD.EfCore.Extensions.Tests")]

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfraSetup
    {
        #region Fields

        private const string RepoConst = "Repo";

        #endregion Fields

        #region Methods

        public static IServiceCollection AddBoundedContexts(this IServiceCollection service, string connectionString)
                    => service
                .AddBoundedContext<ProfileContext>(connectionString, entity => entity.Namespace.Contains(DomainConsts.NamespaceProfile, StringComparison.OrdinalIgnoreCase))
                .AddBoundedContext<AccountContext>(connectionString, entity => entity.Namespace.Contains(DomainConsts.NamespaceAccount, StringComparison.OrdinalIgnoreCase));

        public static IServiceCollection AddRepos(this IServiceCollection service, string connectionString)
                    => service
                .AddBoundedContexts(connectionString)
                .AddRepositories();

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

        private static IServiceCollection AddBoundedContext<TContext>(this IServiceCollection service, string connectionString, Expression<Func<Type, bool>> entityFilter) where TContext : DbContext
        {
            return service.AddDbContext<TContext>(
                op =>
                {
                    op.UseSqlServer(connectionString, o => o.MigrationsHistoryTable(typeof(TContext).Name, DomainSchemas.Migration))
                            .UseAutoConfigModel(o =>
                                o.ScanFrom(typeof(DeleteType).Assembly, typeof(DbContextBase).Assembly)
                                    .WithFilter(entityFilter)
                                    .IgnoreOthers()
                            );
#if DEBUG
                    op.EnableDetailedErrors()
                        .EnableSensitiveDataLogging();
#endif
                }, ServiceLifetime.Scoped, ServiceLifetime.Singleton);
        }

        private static IServiceCollection AddRepositories(this IServiceCollection service)
            => service.AsImplementedInterfaces(typeof(InfraSetup).Assembly.ScanClassesWithFilter(RepoConst));

        #endregion Methods
    }
}