using System;
using System.Collections.Generic;
using System.Reflection;
using HBD.Framework.Extensions;

namespace HBD.EfCore.Hooks
{
    public class HookOptions
    {
        #region Private Fields

        internal  HashSet<IHook> HooksInstance { get; } = new HashSet<IHook>();
        internal  HashSet<Type> HooksTypes  { get; }= new HashSet<Type>();

        #endregion Private Fields

        #region Public Methods

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
            var types = assemblies.Extract().Class().NotAbstract().IsInstanceOf<IHook>();
            HooksTypes.AddRange(types);

            return this;
        }

        #endregion Public Methods
    }
}