using System;
using System.Linq.Expressions;

namespace HBD.EfCore.Extensions.OrderBuilders
{
    public interface IThenOrderByBuilder<T> : IQueryBuilder<T> where T : class
    {
        #region Public Methods

        IThenOrderByBuilder<T> ThenBy(Expression<Func<T, dynamic>> orderBy);

        IThenOrderByBuilder<T> ThenByDescending(Expression<Func<T, dynamic>> orderBy);

        IThenOrderByBuilder<T> ThenBy(string orderBy);

        IThenOrderByBuilder<T> ThenByDescending(string orderBy);

        #endregion Public Methods
    }
}