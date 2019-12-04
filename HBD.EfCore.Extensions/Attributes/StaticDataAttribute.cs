using System;

namespace HBD.EfCore.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class StaticDataAttribute : Attribute
    {
        #region Properties

        public string Schema { get; set; }

        public string Table { get; set; }

        #endregion Properties
    }
}