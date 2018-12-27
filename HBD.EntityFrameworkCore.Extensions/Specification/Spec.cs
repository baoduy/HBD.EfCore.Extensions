using System;
using System.Linq;
using System.Linq.Expressions;

// ReSharper disable MemberCanBeProtected.Global

// ReSharper disable ConvertIfStatementToReturnStatement

namespace HBD.EntityFrameworkCore.Extensions.Specification
{
    /// <summary>
    ///     The Spec for Entity query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Spec<T>
    {
        #region Public Fields

        public static readonly Spec<T> All = new TrueSpec<T>();

        #endregion Public Fields

        #region Private Fields

        private Func<T, bool> _compiledFunc;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        ///     And condition with the other spec
        /// </summary>
        /// <param name="otherSpec"></param>
        /// <returns></returns>
        public virtual Spec<T> And(Spec<T> otherSpec)
        {
            if (otherSpec == null) throw new ArgumentNullException(nameof(otherSpec));

            if (this == All)
                return otherSpec;

            if (otherSpec == All)
                return this;

            return new AndSpec<T>(this, otherSpec);
        }

        /// <summary>
        /// </summary>
        /// <param name="otherSpec"></param>
        /// <returns></returns>
        public virtual Spec<T> ButNot(Spec<T> otherSpec)
        {
            if (otherSpec == null) throw new ArgumentNullException(nameof(otherSpec));

            if (this == All)
                return new NotSpec<T>(otherSpec);

            if (otherSpec == All)
                return this;

            return new AndSpec<T>(this, otherSpec.NotMe());
        }

        /// <summary>
        ///     The specification of including navigation properties of query
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> Includes(IQueryable<T> query) => query;

        /// <summary>
        ///     Check whether the entity satisfy this spec or not. a Condition of Spec should be fully
        ///     defined and not allow to changes after Initialized.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsSatisfied(T entity)
        {
            if (_compiledFunc == null)
                _compiledFunc = ToExpression().Compile();
            return _compiledFunc.Invoke(entity);
        }

        /// <summary>
        ///     Opposite the spec.
        /// </summary>
        /// <returns></returns>
        public virtual Spec<T> NotMe() => new NotSpec<T>(this);

        /// <summary>
        ///     Or condition with the other spec
        /// </summary>
        /// <param name="otherSpec"></param>
        /// <returns></returns>
        public virtual Spec<T> Or(Spec<T> otherSpec)
        {
            if (otherSpec == null) throw new ArgumentNullException(nameof(otherSpec));

            if (this == All || otherSpec == All)
                return All;

            return new OrSpec<T>(this, otherSpec);
        }

        /// <summary>
        /// Provides condition expression for <see cref="IQueryable{T}"/>
        /// </summary>
        /// <returns></returns>
        public abstract Expression<Func<T, bool>> ToExpression();

        #endregion Public Methods
    }
}