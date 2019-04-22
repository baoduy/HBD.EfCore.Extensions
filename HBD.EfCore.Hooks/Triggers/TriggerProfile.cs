using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.EfCore.Hooks.Triggers
{
    public interface ITriggerProfile
    {
        Type EntityType { get; }
        TriggerType TriggerType { get; }
    }

    public abstract class TriggerProfile<T> : ITriggerProfile where T : class
    {
        private readonly TriggerType _triggerType;
        Type ITriggerProfile.EntityType => typeof(T);

        TriggerType ITriggerProfile.TriggerType => _triggerType;

        protected TriggerProfile(TriggerType triggerType) => _triggerType = triggerType;

        internal Task Execute(IEnumerable<dynamic> entities, ITriggerContext context)
            => Execute(entities.OfType<TriggerEntityState<T>>().ToList(), context);

        protected abstract Task Execute(IEnumerable<TriggerEntityState<T>> entities, ITriggerContext context);
    }
}
