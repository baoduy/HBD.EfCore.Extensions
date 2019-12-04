using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HBD.EfCore.Hooks.Tests.Entities;
using HBD.EfCore.Hooks.Triggers;

namespace HBD.EfCore.Hooks.Tests.Providers
{
    internal class UserTriggerAll : UserTrigger
    {
        #region Constructors

        public UserTriggerAll() : base(TriggerTypes.All)
        {
        }

        #endregion Constructors

        #region Properties

        public bool HasFirstNameChanged { get; private set; }

        public bool HasServiceProvider { get; private set; }

        #endregion Properties

        #region Methods

        public override void Reset()
        {
            base.Reset();

            HasServiceProvider = false;
        }

        protected override async Task Execute(IEnumerable<TriggerEntityState<User>> entities, ITriggerContext context)
        {
            await base.Execute(entities, context).ConfigureAwait(false);

            HasServiceProvider = context.ServiceProvider != null;
            HasFirstNameChanged = entities.Any(e => e.HasChangedOn(t => t.FirstName));
        }

        #endregion Methods
    }
}