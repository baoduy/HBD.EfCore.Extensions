using HBD.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HBD.EfCore.Hooks
{
    public class HookOptions
    {
        #region Properties

        internal HashSet<IHook> HooksInstance { get; } = new HashSet<IHook>();

        internal HashSet<Type> HooksTypes { get; } = new HashSet<Type>();

        #endregion Properties

        #region Methods

        public HookOptions Add<THookProvider>() where THookProvider : IHook
        {
            HooksTypes.Add(typeof(THookProvider));
            return this;
        }

        public HookOptions Add(IHook provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            HooksInstance.Add(provider);
            return this;
        }

        public HookOptions ScanFrom(params Assembly[] assemblies)
        {
            var types = assemblies.Extract().Class().NotAbstract().IsInstanceOf<IHook>().Distinct();
            HooksTypes.AddRange(types);

            return this;
        }

        #endregion Methods
    }
}