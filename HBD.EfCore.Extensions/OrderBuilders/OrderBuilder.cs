using System;
using System.Linq;
using System.Linq.Expressions;

namespace HBD.EfCore.Extensions.OrderBuilders
{
    public static class OrderBuilder
    {
        #region Public Methods

        public static IOrderByBuilder<T> CreateBuilder<T>() where T : class => OrderByBuilder<T>.Create();

        public static IThenOrderByBuilder<T> CreateBuilder<T>(Expression<Func<T, object>> orderBy) where T : class =>
            OrderByBuilder<T>.Create().OrderBy(orderBy);

        public static IThenOrderByBuilder<T> CreateBuilder<T>(string orderBy) where T : class =>
            OrderByBuilder<T>.Create().OrderBy(orderBy);

        #endregion Public Methods

        public static IQueryable<T> OrderWith<T>(this IQueryable<T> query, IQueryBuilder<T> order) where T : class => order.Build(query);
    }
}
