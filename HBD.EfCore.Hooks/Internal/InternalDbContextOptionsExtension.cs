using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HBD.EfCore.Hooks.Internal
{
    internal abstract class InternalDbContextOptionsExtension : IDbContextOptionsExtension
    {
        #region Fields

        private DbContextOptionsExtensionInfo info;

        #endregion Fields

        #region Properties

        public DbContextOptionsExtensionInfo Info => info ??= new InternalDbContextOptionsExtensionInfo(this);

        #endregion Properties

        #region Methods

        public virtual void ApplyServices(IServiceCollection services)
        {
        }

        public virtual void Validate(IDbContextOptions options)
        {
        }

        #endregion Methods
    }
}