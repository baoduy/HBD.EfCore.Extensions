using System;
using System.Linq.Expressions;

namespace HBD.EfCore.Extensions.Utilities
{
    public enum OrderingDirection
    {
        Asc = 1,
        Desc = 2
    }

    internal sealed class OrderInfo<T> where T : class
    {
        #region Properties

        public OrderingDirection Direction { get; set; }

        public Expression<Func<T, dynamic>> OrderProperty { get; set; }

        public string OrderPropertyString { get; set; }

        #endregion Properties
    }
}