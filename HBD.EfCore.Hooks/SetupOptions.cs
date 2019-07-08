using System;
using System.Linq;
using System.Reflection;

namespace HBD.EfCore.Hooks
{
    public sealed class SetupOptions
    {
        internal bool DeepValidation { get; private set; } = true;
        internal bool SavingAwareness { get; private set; } = true;
        internal bool ValidateAllProperties { get; private set; } = false;
        internal bool Trigger { get; private set; } = false;
        internal Assembly[] Assemblies { get; private set; }

        public SetupOptions UseDeepValidation(bool flag = true,bool validateAllProperties = true)
        {
            DeepValidation = flag;
            return this;
        }

        public SetupOptions UseDeepSavingAwareness(bool flag = true)
        {
            SavingAwareness = flag;
            return this;
        }

        public SetupOptions UseTrigger(bool flag = true)
        {
            Trigger = flag;
            return this;
        }

        public SetupOptions ScanFrom(params Assembly[] assemblies)
        {
            if (assemblies == null || !assemblies.Any())
                throw new ArgumentNullException(nameof(assemblies));

            Assemblies = assemblies;
            return this;
        }
    }
}
