using System;
using System.Linq.Expressions;

namespace HBD.EfCore.Extensions.OrderBuilders
{
    public interface IOrderByBuilder<T> : IQueryBuilder<T> where T : class
    {
        #region Public Methods

        IThenOrderByBuilder<T> OrderBy(Expression<Func<T, dynamic>> orderBy);

        IThenOrderByBuilder<T> OrderByDescending(Expression<Func<T, dynamic>> orderBy);

        IThenOrderByBuilder<T> OrderBy(string orderBy);

        IThenOrderByBuilder<T> OrderByDescending(string orderBy);

        #endregion Public Methods
    }
}