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

    public abstract class TriggerProfile<T> : ITriggerProfile
    {
        private readonly TriggerType _triggerType;
        Type ITriggerProfile.EntityType => typeof(T);

        TriggerType ITriggerProfile.TriggerType => _triggerType;

        protected TriggerProfile(TriggerType triggerType) => _triggerType = triggerType;

        internal Task Execute(IReadOnlyCollection<dynamic> entities, TriggerContext context)
            => Execute(entities.OfType<T>().ToList(), context);

        protected abstract Task Execute(IReadOnlyCollection<T> entities, TriggerContext context);
    }
}
