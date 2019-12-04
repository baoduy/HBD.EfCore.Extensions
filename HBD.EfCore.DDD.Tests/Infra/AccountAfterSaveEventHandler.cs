using System.Threading.Tasks;
using GenericEventRunner.ForEntities;
using GenericEventRunner.ForHandlers;

namespace HBD.EfCore.DDD.Tests.Infra
{
    internal class AccountAfterSaveEventHandler : IAfterSaveEventHandler<AccountEvent>, IAfterSaveEventHandlerAsync<AccountEvent>
    {
        #region Properties

        public static bool Called { get; private set; }

        #endregion Properties

        #region Methods

        public static void Reset() => Called = false;

        public void Handle(EntityEvents callingEntity, AccountEvent domainEvent) => Called = true;

        public Task HandleAsync(EntityEvents callingEntity, AccountEvent domainEvent)
        {
            Called = true;
            return Task.CompletedTask;
        }

        #endregion Methods
    }
}