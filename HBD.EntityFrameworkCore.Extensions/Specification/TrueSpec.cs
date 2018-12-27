using System;
using System.Linq.Expressions;

namespace HBD.EntityFrameworkCore.Extensions.Specification
{
    /// <inheritdoc/>
    internal sealed class TrueSpec<T> : Spec<T>
    {
        #region Public Methods

        public override Expression<Func<T, bool>> ToExpression() => x => true;

        #endregion Public Methods
    }
}