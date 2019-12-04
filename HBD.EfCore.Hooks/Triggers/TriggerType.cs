using System;

namespace HBD.EfCore.Hooks.Triggers
{
    [Flags]
    public enum TriggerTypes
    {
        None = 0,
        Created = 1,
        Updated = 2,
        Deleted = 4,
        All = Created | Updated | Deleted
    }
}