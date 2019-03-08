using System;

namespace HBD.EfCore.Hooks.Triggers
{
    [Flags]
    public enum TriggerType
    {
        None = 0,
        Created = 1,
        Updated = 2,
        Deleted = 4,
        All = Created | Updated | Deleted
    }
}
