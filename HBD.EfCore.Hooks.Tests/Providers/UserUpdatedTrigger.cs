using HBD.EfCore.Hooks.Triggers;

namespace HBD.EfCore.Hooks.Tests.Providers
{
    internal class UserUpdatedTrigger : UserTrigger
    {
        #region Constructors

        public UserUpdatedTrigger() : base(TriggerTypes.Updated)
        {
        }

        #endregion Constructors
    }
}