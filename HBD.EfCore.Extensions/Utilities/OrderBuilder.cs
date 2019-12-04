using HBD.EfCore.Extensions.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore
{
    public static class OrderBuilder
    {
        #region Methods

        public static IThenOrderByBuilder<T> CreateBuilder<T>(Expression<Func<T, object>> orderBy) where T : class =>
            CreateBuilder<T>().OrderBy(orderBy);

        public static IThenOrderByBuilder<T> CreateBuilder<T>(string orderBy) where T : class =>
            CreateBuilder<T>().OrderBy(orderBy);

        public static IQueryable<T> OrderWith<T>(this IQueryable<T> query, IQueryBuilder<T> order) where T : class
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            return order.Build(query);
        }

        internal static IOrderByBuilder<T> CreateBuilder<T>() where T : class => new OrderByBuilder<T>();

        #endregion Methods
    }
}