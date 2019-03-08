using System;
using HBD.EfCore.Extensions.Internal;
using HBD.EfCore.Extensions.Options;
using Microsoft.EntityFrameworkCore.Infrastructure;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class SetupServiceExtensions
    {
        #region Public Methods

        public static DbContextOptionsBuilder<TContext> UseAutoConfigModel<TContext>(
            this DbContextOptionsBuilder<TContext> @this,
            Action<IEntityMappingExtension> options = null)
            where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>)((DbContextOptionsBuilder)@this)
                .UseAutoConfigModel(options);

        public static DbContextOptionsBuilder UseAutoConfigModel(this DbContextOptionsBuilder @this,
            Action<IEntityMappingExtension> options = null)
        {
            var op = @this.GetOrCreateExtension();
            options?.Invoke(op);

            return @this;
        }

        #endregion Public Methods

        #region Private Methods

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

        #endregion Private Methods
    }
}