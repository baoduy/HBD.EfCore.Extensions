using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HBD.EfCore.Hooks
{
    public sealed class SetupOptions
    {
        #region Properties

        internal ICollection<Assembly> Assemblies { get; private set; }

        internal bool DeepValidation { get; private set; } = true;

        internal bool SavingAwareness { get; private set; } = true;

        internal bool Trigger { get; private set; } = false;

        internal bool ValidateAllProperties { get; private set; } = false;

        #endregion Properties

        #region Methods

        public SetupOptions ScanFrom(params Assembly[] assemblies)
        {
            if (assemblies == null || !assemblies.Any())
                throw new ArgumentNullException(nameof(assemblies));

            Assemblies = assemblies;
            return this;
        }

        public SetupOptions UseDeepSavingAwareness(bool flag = true)
        {
            SavingAwareness = flag;
            return this;
        }

        public SetupOptions UseDeepValidation(bool flag = true)
        {
            DeepValidation = flag;
            return this;
        }

        public SetupOptions UseTrigger(bool flag = true)
        {
            Trigger = flag;
            return this;
        }

        #endregion Methods
    }
}