using System;
using System.Linq;
using System.Linq.Expressions;

namespace HBD.EntityFrameworkCore.Extensions.Specification
{
    /// <inheritdoc/>
    internal sealed class NotSpec<T> : Spec<T>
    {
        #region Private Fields

        private readonly Spec<T> _specification;

        #endregion Private Fields

        #region Public Constructors

        public NotSpec(Spec<T> specification) => _specification = specification;

        #endregion Public Constructors

        #region Public Methods

        public override IQueryable<T> Includes(IQueryable<T> query)
            => _specification.Includes(query);

        public override Expression<Func<T, bool>> ToExpression()
        {
            var expression = _specification.ToExpression();
            var notExpression = Expression.Not(expression.Body);

            return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters);
        }

        #endregion Public Methods
    }
}