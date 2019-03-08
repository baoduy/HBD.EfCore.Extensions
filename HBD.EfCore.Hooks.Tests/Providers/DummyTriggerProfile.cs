using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HBD.EfCore.Hooks.Tests.Entities;
using HBD.EfCore.Hooks.Triggers;

namespace HBD.EfCore.Hooks.Tests.Providers
{
    public class DummyTriggerProfile : TriggerProfile<User>
    {
        public bool HasEntity { get; private set; }
        public bool HasServiceProvider { get; private set; }
        public bool Called { get; private set; }

        public void Reset()
        {
            Called = false;
            HasEntity = false;
            HasServiceProvider = false;
        }

        protected override Task Execute(IReadOnlyCollection<User> entities, TriggerContext context)
        {
            Called = true;

            HasEntity = entities.Any();
            HasServiceProvider = context.ServiceProvider != null;
            return Task.CompletedTask;
        }

        public DummyTriggerProfile() : base(TriggerType.All)
        {
        }
    }
}
