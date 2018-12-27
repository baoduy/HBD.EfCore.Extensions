using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace HBD.EntityFrameworkCore.Extensions.Specification
{
    /// <inheritdoc/>
    internal sealed class OrSpec<T> : Spec<T>
    {
        #region Private Fields

        private readonly Spec<T> _left;
        private readonly Spec<T> _right;

        #endregion Private Fields

        #region Public Constructors

        public OrSpec(Spec<T> left, Spec<T> right)
        {
            _right = right;
            _left = left;
        }

        #endregion Public Constructors

        #region Public Methods

        public override IQueryable<T> Includes(IQueryable<T> query)
            => query.Includes(_left).Includes(_right);

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();

            var orExpression = Expression.OrElse(leftExpression.Body, Expression.Invoke(rightExpression, leftExpression.Parameters[0]));

            return Expression.Lambda<Func<T, bool>>(orExpression, leftExpression.Parameters);
        }

        #endregion Public Methods
    }
}