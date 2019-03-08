using HBD.EfCore.EntityResolver.Attributes;
using System.Reflection;

namespace HBD.EfCore.EntityResolver.Internal
{
    public sealed class ResolverPropertyInfo
    {
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

        public PropertyInfo Property { get; }

        public AutoResolveAttribute Attribute { get; }

        public bool AlwaysIncluded { get; private set; }
    }
}
