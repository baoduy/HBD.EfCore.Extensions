using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HBD.EfCore.Hooks.Internal
{
    internal class InternalDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        #region Constructors

        public InternalDbContextOptionsExtensionInfo(InternalDbContextOptionsExtension extension) : base(extension)
        {
        }

        #endregion Constructors

        #region Properties

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => $"use {this.Extension.GetType().Name}";

        #endregion Properties

        #region Methods

        public override long GetServiceProviderHashCode() => (this.Extension.GetType().FullName.GetHashCode(StringComparison.Ordinal) * 3) ^ this.GetHashCode();

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            if (debugInfo is null)
                throw new ArgumentNullException(nameof(debugInfo));

            debugInfo["Core:" + this.Extension.GetType().Name] = this.GetServiceProviderHashCode().ToString(CultureInfo.InvariantCulture);
        }

        #endregion Methods
    }
}