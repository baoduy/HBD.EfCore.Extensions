using System;

namespace HBD.EfCore.Hooks.Triggers
{
    public sealed class TriggerContext
    {
        public IServiceProvider ServiceProvider { get; }

        internal TriggerContext(IServiceProvider serviceProvider) 
            => ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }
}
