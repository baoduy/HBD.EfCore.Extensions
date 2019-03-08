using System;

namespace HBD.EfCore.EntityResolver.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Always include the property value to dynamic object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AlwaysIncludedAttribute : Attribute
    {
        public bool AlwaysIncluded { get; set; } = true;
    }
}
