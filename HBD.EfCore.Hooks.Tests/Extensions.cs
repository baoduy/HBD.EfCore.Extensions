using HBD.EfCore.Hooks.Tests.Providers;
using System.Collections.Generic;

namespace HBD.EfCore.Hooks.Tests
{
    internal static class Extensions
    {
        #region Methods

        public static void ResetAll(this IList<UserTrigger> profiles)
        {
            foreach (var p in profiles)
                p.Reset();
        }

        #endregion Methods
    }
}