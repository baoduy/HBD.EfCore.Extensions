using HBD.EfCore.Extensions.Pageable;
using HBD.EfCore.Extensions.Specification;
using System;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class SpecExtensions
    {
        #region Methods

        /// <summary>
        ///     Create a Pageable to PageableSpec
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSpec"></typeparam>
        /// <param name="this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IPageable<T> ForPageSpec<T, TSpec>(this DbContext @this, params object[] args) where T : class where TSpec : PageableSpec<T>
            => @this.ForPageSpec(CreateInstance<T, TSpec>(args));

        /// <summary>
        ///     Create a Paging-able to PageableSpec
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public static IPageable<T> ForPageSpec<T>(this DbContext @this, PageableSpec<T> spec) where T : class
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return @this.Set<T>().ToPageable(spec);
        }

        /// <summary>
        ///     Create a Paging-able to PageableSpec
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSpec"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static Task<IPageable<T>> ForPageSpecAsync<T, TSpec>(this DbContext @this, params object[] args) where T : class where TSpec : PageableSpec<T>
            => @this.ForPageSpecAsync(CreateInstance<T, TSpec>(args));

        /// <summary>
        ///     Create a Pageable to PageableSpec
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public static Task<IPageable<T>> ForPageSpecAsync<T>(this DbContext @this, PageableSpec<T> spec) where T : class
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return @this.Set<T>().ToPageableAsync(spec);
        }

        /// <summary>
        ///     Create query for a Spec
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSpec"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IQueryable<T> ForSpec<T, TSpec>(this DbContext @this, params object[] args) where T : class where TSpec : Spec<T>
            => @this.ForSpec(CreateInstance<T, TSpec>(args));

        /// <summary>
        ///     Create query for a Spec
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public static IQueryable<T> ForSpec<T>(this DbContext @this, Spec<T> spec) where T : class
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return @this.Set<T>().ForSpec(spec);
        }

        /// <summary>
        ///     Add ONLY Including of Spec to query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSpec"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IQueryable<T> Includes<T, TSpec>(this IQueryable<T> @this, params object[] args) where TSpec : Spec<T>
            => @this.Includes(CreateInstance<T, TSpec>(args));

        /// <summary>
        ///     Add ONLY Including of Spec to query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public static IQueryable<T> Includes<T>(this IQueryable<T> @this, Spec<T> spec)
            => spec == null ? @this : spec.Includes(@this);

        /// <summary>
        ///     Add ONLY expression condition of Spec to query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSpec"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IQueryable<T> Wheres<T, TSpec>(this IQueryable<T> @this, params object[] args) where TSpec : Spec<T>
            => @this.Wheres(CreateInstance<T, TSpec>(args));

        /// <summary>
        ///     Add ONLY expression condition of Spec to query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public static IQueryable<T> Wheres<T>(this IQueryable<T> @this, Spec<T> spec)
            => spec == null ? @this : @this.Where(spec.ToExpression());

        private static TSpec CreateInstance<T, TSpec>(params object[] args) where TSpec : Spec<T> => (TSpec)Activator.CreateInstance(typeof(TSpec), args);

        private static IQueryable<T> ForSpec<T>(this IQueryable<T> query, Spec<T> spec) where T : class
        {
            switch (spec)
            {
                case null:
                    return query;

                case PageableSpec<T> _:
                    throw new ArgumentException(
                        "The Spec is a PageableSpec. Please use ForPageSpec extension instead.");
                default:
                    return query.Includes(spec).Wheres(spec);
            }
        }

        #endregion Methods
    }
}