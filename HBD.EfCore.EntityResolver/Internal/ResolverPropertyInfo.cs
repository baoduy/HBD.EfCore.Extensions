using HBD.EfCore.EntityResolvers.Attributes;
using System.Reflection;

namespace HBD.EfCore.EntityResolvers.Internal
{
    public sealed class ResolverPropertyInfo
    {
        #region Constructors

        internal ResolverPropertyInfo(PropertyInfo property, AutoResolveAttribute attribute)
        {
            Property = property;
            Attribute = attribute;
        }

        internal ResolverPropertyInfo(PropertyInfo property, AlwaysIncludedAttribute attribute)
        {
            Property = property;
            AlwaysIncluded = attribute?.AlwaysIncluded ?? false;
        }

        #endregion Constructors

        #region Properties

        public bool AlwaysIncluded { get; private set; }

        public AutoResolveAttribute Attribute { get; }

        public PropertyInfo Property { get; }

        #endregion Properties
    }
}