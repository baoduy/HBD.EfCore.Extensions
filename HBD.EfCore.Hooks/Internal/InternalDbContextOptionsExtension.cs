using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HBD.EfCore.Hooks.Internal
{
    internal abstract class InternalDbContextOptionsExtension : IDbContextOptionsExtension
    {
        #region Public Properties

        public string LogFragment => $"use {this.GetType().Name}";

        #endregion Public Properties

        #region Public Methods

        public virtual bool ApplyServices(IServiceCollection services) => false;

        public virtual long GetServiceProviderHashCode() => this.GetType().Name.GetHashCode();

        public virtual void Validate(IDbContextOptions options)
        {
        }

        #endregion Public Methods
    }
}