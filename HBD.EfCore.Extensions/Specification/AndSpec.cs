using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace HBD.EfCore.Extensions.Specification
{
    internal sealed class AndSpec<T> : Spec<T>
    {
        #region Fields

        private readonly Spec<T> _left;
        private readonly Spec<T> _right;

        #endregion Fields

        #region Constructors

        public AndSpec(Spec<T> left, Spec<T> right)
        {
            _right = right;
            _left = left;
        }

        #endregion Constructors

        #region Methods

        public override IQueryable<T> Includes(IQueryable<T> query)
            => query.Includes(_left).Includes(_right);

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();

            return leftExpression.And(rightExpression);
        }

        #endregion Methods
    }
}