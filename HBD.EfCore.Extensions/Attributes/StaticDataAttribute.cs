using System;

namespace HBD.EfCore.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class StaticDataAttribute : Attribute
    {
        public string Table { get; set; }
        public string Schema { get; set; }
    }
}