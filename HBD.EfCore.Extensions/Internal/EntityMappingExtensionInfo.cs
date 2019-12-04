using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HBD.EfCore.Extensions.Internal
{
    internal sealed class EntityMappingExtensionInfo : DbContextOptionsExtensionInfo
    {
        #region Constructors

        public EntityMappingExtensionInfo(EntityMappingExtension extension) : base(extension)
        {
        }

        #endregion Constructors

        #region Properties

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => $"using {nameof(EntityMappingExtension)}";

        #endregion Properties

        #region Methods

        public override long GetServiceProviderHashCode() => (nameof(EntityMappingExtension).GetHashCode() * 3) ^ this.GetHashCode();

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            if (debugInfo is null)
                throw new ArgumentNullException(nameof(debugInfo));

            debugInfo["Core:" + nameof(EntityMappingExtension)] = this.GetServiceProviderHashCode().ToString(CultureInfo.InvariantCulture);
        }

        #endregion Methods
    }
}