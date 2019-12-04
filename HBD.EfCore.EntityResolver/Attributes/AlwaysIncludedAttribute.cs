using System;

namespace HBD.EfCore.EntityResolvers.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Always include the property value to dynamic object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AlwaysIncludedAttribute : Attribute
    {
        #region Properties

        public bool AlwaysIncluded { get; set; } = true;

        #endregion Properties
    }
}