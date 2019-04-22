using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks.Triggers
{
    [DebuggerDisplay("{Entity.Name} {State}")]
    public sealed class TriggerEntityState
    {
        public TriggerEntityState(object entity, IEnumerable<string> modifiedProperties, Type entityType, EntityState state)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
            State = state;
            ModifiedProperties = new List<string>(modifiedProperties);
        }

        public object Entity { get; }
        public Type EntityType { get; }
        public EntityState State { get; }
        public IReadOnlyCollection<string> ModifiedProperties { get; }
    }

    public sealed class TriggerEntityState<T> where T : class
    {
        public TriggerEntityState(T entity, IEnumerable<string> modifiedProperties, EntityState state)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            State = state;
            ModifiedProperties = new List<string>(modifiedProperties);
        }

        public T Entity { get; }
        public EntityState State { get; }
        public IReadOnlyCollection<string> ModifiedProperties { get; }
    }
}
