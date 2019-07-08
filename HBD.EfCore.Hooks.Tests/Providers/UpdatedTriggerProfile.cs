using HBD.EfCore.Hooks.Tests.Entities;
using HBD.EfCore.Hooks.Triggers;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBD.EfCore.Hooks.Tests.Providers
{
    internal class UpdatedTriggerProfile : TriggerProfile<User>
    {
        #region Public Constructors

        public UpdatedTriggerProfile() : base(TriggerType.Updated)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public bool IsCalled { get; private set; }

        public bool IsEmpty { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Reset()
        {
            IsCalled = false;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override Task Execute(IEnumerable<TriggerEntityState<User>> entities, ITriggerContext context)
        {
            IsCalled = true;
            IsEmpty = !entities.Any();

            return Task.CompletedTask;
        }

        #endregion Protected Methods
    }
}