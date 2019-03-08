using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HBD.Framework.Extensions;
using Remotion.Linq.Clauses;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace HBD.EfCore.Extensions.OrderBuilders
{
    /// <summary>
    /// Implemented Query Builder
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class OrderByBuilder<T> : IOrderByBuilder<T>, IThenOrderByBuilder<T> where T : class
    {
        #region Internal Constructors

        private OrderByBuilder() => ThenByProps = new List<OrderInfo<T>>();

        #endregion Internal Constructors

        #region Private Methods

        private static IQueryable<T> ApplyInfo(IQueryable<T> query, OrderInfo<T> orderInfo, bool ordered = false)
        {
            if (orderInfo == null) return query;

            if (typeof(IOrderedQueryable<T>) == query.Expression.Type)
            {
                var orderedQuery = (IOrderedQueryable<T>)query;

                if (orderInfo.OrderProperty != null)
                {
                    return orderInfo.Direction == OrderingDirection.Asc
                        ? orderedQuery.ThenBy(orderInfo.OrderProperty)
                        : orderedQuery.ThenByDescending(orderInfo.OrderProperty);
                }

                if (orderInfo.OrderPropertyString.IsNotNullOrEmpty())
                {
                    return orderInfo.Direction == OrderingDirection.Asc
                        ? orderedQuery.ThenByDynamic(orderInfo.OrderPropertyString)
                        : orderedQuery.ThenByDescendingDynamic(orderInfo.OrderPropertyString);
                }
            }
            else
            {
                if (orderInfo.OrderProperty != null)
                {
                    return orderInfo.Direction == OrderingDirection.Asc
                        ? query.OrderBy(orderInfo.OrderProperty)
                        : query.OrderByDescending(orderInfo.OrderProperty);
                }

                if (orderInfo.OrderPropertyString.IsNotNullOrEmpty())
                {
                    return orderInfo.Direction == OrderingDirection.Asc
                        ? query.OrderByDynamic(orderInfo.OrderPropertyString)
                        : query.OrderByDescendingDynamic(orderInfo.OrderPropertyString);
                }
            }

            return query;
        }

        #endregion Private Methods

        #region Private Properties

        private OrderInfo<T> OrderByProp { get; set; }

        private IList<OrderInfo<T>> ThenByProps { get; }

        #endregion Private Properties

        #region Public Methods

        public static IOrderByBuilder<T> Create() => new OrderByBuilder<T>();

        public virtual IQueryable<T> Build(IQueryable<T> query)
        {
            query = ApplyInfo(query, OrderByProp);
            return ThenByProps.Aggregate(query, (q, i) => ApplyInfo(q, i, true));
        }

        public IThenOrderByBuilder<T> OrderBy(Expression<Func<T, dynamic>> orderBy)
        {
            OrderByProp = new OrderInfo<T>
            {
                Direction = OrderingDirection.Asc,
                OrderProperty = orderBy
            };

            return this;
        }

        public IThenOrderByBuilder<T> OrderBy(string orderBy)
        {
            OrderByProp = new OrderInfo<T>
            {
                Direction = OrderingDirection.Asc,
                OrderPropertyString = orderBy
            };

            return this;
        }

        public IThenOrderByBuilder<T> OrderByDescending(Expression<Func<T, dynamic>> orderBy)
        {
            OrderByProp = new OrderInfo<T>
            {
                Direction = OrderingDirection.Desc,
                OrderProperty = orderBy
            };

            return this;
        }

        public IThenOrderByBuilder<T> OrderByDescending(string orderBy)
        {
            OrderByProp = new OrderInfo<T>
            {
                Direction = OrderingDirection.Desc,
                OrderPropertyString = orderBy
            };

            return this;
        }

        public IThenOrderByBuilder<T> ThenBy(Expression<Func<T, dynamic>> orderBy)
        {
            ThenByProps.Add(new OrderInfo<T>
            {
                Direction = OrderingDirection.Asc,
                OrderProperty = orderBy
            });

            return this;
        }

        public IThenOrderByBuilder<T> ThenBy(string orderBy)
        {
            ThenByProps.Add(new OrderInfo<T>
            {
                Direction = OrderingDirection.Asc,
                OrderPropertyString = orderBy
            });

            return this;
        }

        public IThenOrderByBuilder<T> ThenByDescending(Expression<Func<T, dynamic>> orderBy)
        {
            ThenByProps.Add(new OrderInfo<T>
            {
                Direction = OrderingDirection.Desc,
                OrderProperty = orderBy
            });

            return this;
        }

        public IThenOrderByBuilder<T> ThenByDescending(string orderBy)
        {
            ThenByProps.Add(new OrderInfo<T>
            {
                Direction = OrderingDirection.Desc,
                OrderPropertyString = orderBy
            });

            return this;
        }

        #endregion Public Methods
    }
}