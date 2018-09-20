using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions;
using HBD.EntityFrameworkCore.Extensions.Internal;

// ReSharper disable CheckNamespace

namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static ITypeExtractor Extract(this Assembly assembly)
            => new TypeExtractor(assembly);

        public static ITypeExtractor Extract(this Assembly[] assemblies)
            => new TypeExtractor(assemblies);

        public static DbContextOptionsBuilder<TContext> RegisterEntities<TContext>(this DbContextOptionsBuilder<TContext> @this, Action<EntityAutoMappingDbExtension> options = null) where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>)((DbContextOptionsBuilder)@this).RegisterEntities(options);

        public static DbContextOptionsBuilder RegisterEntities(this DbContextOptionsBuilder @this, Action<EntityAutoMappingDbExtension> options = null)
        {
            var op = @this.GetOrCreateExtension();
            options?.Invoke(op);
            return @this;
        }

        private static EntityAutoMappingDbExtension GetOrCreateExtension(this DbContextOptionsBuilder optionsBuilder)
        {
            var op = optionsBuilder.Options.FindExtension<EntityAutoMappingDbExtension>();

            if (op == null)
            {
                op = new EntityAutoMappingDbExtension();
                ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(op);
            }

            return op;
        }
    }
}
