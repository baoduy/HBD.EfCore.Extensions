using GenericEventRunner.ForEntities;
using GenericEventRunner.ForHandlers;
using StatusGeneric;

namespace HBD.EfCore.DDD.Tests.Infra
{
    internal class AccountBeforeSaveEventHandler : IBeforeSaveEventHandler<AccountEvent>
    {
        #region Properties

        public static bool Called { get; private set; }

        #endregion Properties

        #region Methods

        public static void Reset() => Called = false;

        public IStatusGeneric Handle(EntityEvents callingEntity, AccountEvent domainEvent)
        {
            Called = true;
            return new StatusGenericHandler();
        }

        #endregion Methods
    }
}