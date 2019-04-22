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
        public bool HasFirstNameChanged { get; private set; }
        public int Called { get; private set; }

        public void Reset()
        {
            Called = 0;
            HasEntity = false;
            HasServiceProvider = false;
        }

        protected override Task Execute(IEnumerable<TriggerEntityState<User>> entities, ITriggerContext context)
        {
            Called += 1;

            HasEntity = entities.Any();
            HasServiceProvider = context.ServiceProvider != null;
            HasFirstNameChanged = entities.Any(e => e.HasChangedOn(t => t.FirstName));

            return Task.CompletedTask;
        }

        public DummyTriggerProfile() : base(TriggerType.All)
        {
        }
    }
}
