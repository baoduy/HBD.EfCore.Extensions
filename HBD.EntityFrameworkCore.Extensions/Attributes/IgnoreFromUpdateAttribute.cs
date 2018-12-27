using System;

namespace HBD.EntityFrameworkCore.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class IgnoreFromUpdateAttribute : Attribute
    {
    }
}