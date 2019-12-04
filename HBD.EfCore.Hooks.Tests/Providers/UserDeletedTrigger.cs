using HBD.EfCore.Hooks.Triggers;

namespace HBD.EfCore.Hooks.Tests.Providers
{
    internal class UserDeletedTrigger : UserTrigger
    {
        #region Constructors

        public UserDeletedTrigger() : base(TriggerTypes.Deleted)
        {
        }

        #endregion Constructors
    }
}