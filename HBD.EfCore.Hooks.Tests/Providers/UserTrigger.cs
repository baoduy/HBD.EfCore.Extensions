using HBD.EfCore.Hooks.Tests.Entities;
using HBD.EfCore.Hooks.Triggers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.EfCore.Hooks.Tests.Providers
{
    internal abstract class UserTrigger : TriggerProfile<User>
    {
        #region Constructors

        protected UserTrigger(TriggerTypes triggerType) : base(triggerType)
        {
        }

        #endregion Constructors

        #region Properties

        public int CalledCount { get; private set; }

        public bool HasEntity { get; private set; }

        #endregion Properties

        #region Methods

        public virtual void Reset()
        {
            CalledCount = 0;
            HasEntity = false;
        }

        protected override Task Execute(IEnumerable<TriggerEntityState<User>> entities, ITriggerContext context)
        {
            CalledCount += 1;
            HasEntity = entities.Any();

            return Task.CompletedTask;
        }

        #endregion Methods
    }
}