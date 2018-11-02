using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using HBD.EntityFrameworkCore.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> RegisterEntities<TContext>(this DbContextOptionsBuilder<TContext> @this, Action<IEntityMappingExtension> options = null) where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>)((DbContextOptionsBuilder)@this).RegisterEntities(options);

        public static DbContextOptionsBuilder RegisterEntities(this DbContextOptionsBuilder @this, Action<IEntityMappingExtension> options = null)
        {
            var op = @this.GetOrCreateExtension();
            options?.Invoke(op);
            return @this;
        }

        private static EntityMappingExtension GetOrCreateExtension(this DbContextOptionsBuilder optionsBuilder)
        {
            var op = optionsBuilder.Options.FindExtension<EntityMappingExtension>();

            if (op == null)
            {
                op = new EntityMappingExtension();
                ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(op);
            }

            return op;
        }
    }
}
