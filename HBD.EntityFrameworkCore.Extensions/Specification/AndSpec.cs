using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace HBD.EntityFrameworkCore.Extensions.Specification
{
    internal sealed class AndSpec<T> : Spec<T>
    {
        #region Private Fields

        private readonly Spec<T> _left;
        private readonly Spec<T> _right;

        #endregion Private Fields

        #region Public Constructors

        public AndSpec(Spec<T> left, Spec<T> right)
        {
            _right = right;
            _left = left;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IQueryable<T> Includes(IQueryable<T> query)
            => query.Includes(_left).Includes(_right);

        #endregion Public Properties

        #region Public Methods

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();

            var andExpression = Expression.AndAlso(leftExpression.Body, Expression.Invoke(rightExpression, leftExpression.Parameters[0]));

            return Expression.Lambda<Func<T, bool>>(andExpression, leftExpression.Parameters);
        }

        #endregion Public Methods
    }
}