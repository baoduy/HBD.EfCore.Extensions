using System;

namespace HBD.EfCore.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class IgnoreFromUpdateAttribute : Attribute
    {
    }
}