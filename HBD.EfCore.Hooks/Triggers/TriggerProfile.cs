using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.EfCore.Hooks.Triggers
{
    public interface ITriggerProfile
    {
        #region Properties

        Type EntityType { get; }

        TriggerTypes TriggerType { get; }

        #endregion Properties
    }

    public abstract class TriggerProfile<T> : ITriggerProfile where T : class
    {
        #region Fields

        private TriggerTypes _triggerType;
        private Type entityType;

        #endregion Fields

        #region Constructors

        protected TriggerProfile(TriggerTypes triggerType)
        {
            _triggerType = triggerType;
            entityType = typeof(T);
        }

        #endregion Constructors

        #region Properties

        Type ITriggerProfile.EntityType => entityType;

        TriggerTypes ITriggerProfile.TriggerType => _triggerType;

        #endregion Properties

        #region Methods

        internal Task Execute(IEnumerable<dynamic> entities, ITriggerContext context)
            => Execute(entities.OfType<TriggerEntityState<T>>().ToList(), context);

        protected abstract Task Execute(IEnumerable<TriggerEntityState<T>> entities, ITriggerContext context);

        #endregion Methods
    }
}