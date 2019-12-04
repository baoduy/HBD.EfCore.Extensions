using HBD.EfCore.Hooks.Triggers;

namespace HBD.EfCore.Hooks.Tests.Providers
{
    internal class UserCreatedTrigger : UserTrigger
    {
        #region Constructors

        public UserCreatedTrigger() : base(TriggerTypes.Created)
        {
        }

        #endregion Constructors
    }
}