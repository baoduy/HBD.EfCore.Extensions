using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using HBD.EntityFrameworkCore.Extensions;

// ReSharper disable CheckNamespace

namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> RegisterEntities<TContext>(this DbContextOptionsBuilder<TContext> @this, Action<EntityAutoMappingDbExtension> options) where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>)((DbContextOptionsBuilder)@this).RegisterEntities(options);

        public static DbContextOptionsBuilder RegisterEntities(this DbContextOptionsBuilder @this, Action<EntityAutoMappingDbExtension> options)
        {
            var op = @this.GetOrCreateExtension();
            options.Invoke(op);
            return @this;
        }

        private static EntityAutoMappingDbExtension GetOrCreateExtension(this DbContextOptionsBuilder optionsBuilder)
        {
            var op = optionsBuilder.Options.FindExtension<EntityAutoMappingDbExtension>();

            if (op == null)
            {
                op = new EntityAutoMappingDbExtension();
                ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension<EntityAutoMappingDbExtension>(op);
            }

            return op;
        }
    }
}
