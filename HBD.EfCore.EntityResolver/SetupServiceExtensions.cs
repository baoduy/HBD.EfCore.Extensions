using HBD.EfCore.EntityResolver;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class SetupServiceExtensions
    {
        #region Public Methods

        public static IServiceCollection AddEntityResolver(this IServiceCollection service)
            => service
                .AddScoped(typeof(IEntityResolverAsync<>), typeof(EntityResolverAsync<>))
                .AddScoped<IEntityResolverAsync, EntityResolverAsync>()
                .AddScoped(typeof(IEntityResolver<>), typeof(EntityResolver<>))
                .AddScoped<IEntityResolver, EntityResolver>();

        #endregion Public Methods
    }
}