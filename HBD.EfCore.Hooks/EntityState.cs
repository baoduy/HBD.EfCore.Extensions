using System;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks
{
    public sealed class EntityInfo
    {
        public EntityInfo(object entity, Type entityType, EntityState state)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
            State = state;
        }

        public object Entity { get;  }
        public Type EntityType { get; }
        public EntityState State { get;  }
    }
}
