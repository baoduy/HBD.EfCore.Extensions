using Microsoft.Extensions.DependencyInjection;
using System;

namespace HBD.EfCore.Hooks.Triggers
{
    public interface ITriggerContext
    {
        IServiceProvider ServiceProvider { get; }
    }

    public sealed class TriggerContext : IDisposable, ITriggerContext
    {
        private readonly IServiceCollection _serviceCollection;
        private ServiceProvider _provider;

        public IServiceProvider ServiceProvider
            => _provider ?? (_provider = _serviceCollection.BuildServiceProvider());

        internal TriggerContext(IServiceCollection serviceCollection)
            => _serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));

        public void Dispose() => _provider?.Dispose();
    }
}
