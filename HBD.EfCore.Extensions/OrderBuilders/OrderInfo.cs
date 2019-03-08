using System;
using System.Linq.Expressions;
using Remotion.Linq.Clauses;

namespace HBD.EfCore.Extensions.OrderBuilders
{
    internal sealed class OrderInfo<T> where T : class
    {
        public Expression<Func<T, dynamic>> OrderProperty { get; set; }

        public string OrderPropertyString { get; set; }

        public OrderingDirection Direction { get; set; }
    }
}
