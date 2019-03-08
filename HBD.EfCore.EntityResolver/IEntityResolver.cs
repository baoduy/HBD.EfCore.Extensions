using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.EntityResolver
{
    public interface IEntityResolver<out TDbDbContext> where TDbDbContext : DbContext
    {
        #region Public Properties

        TDbDbContext DbContext { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Transform The Model
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">The model will be transformed</param>
        /// <param name="ignoreOtherProperties">If = true the result will content the transforming properties only and all other property</param>
        /// <returns></returns>
        dynamic Resolve<TSource>(TSource source, bool ignoreOtherProperties = false) where TSource : class;

        /// <summary>
        /// Transform and map the result to Destination using AutoMapper. Ensure AutoMapper is provided.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="ignoreOtherProperties"></param>
        /// <returns></returns>
        void ResolveAndMap<TSource>(TSource source, object destination, bool ignoreOtherProperties = false)
        where TSource : class;

        /// <summary>
        /// Transform and map the result to Destination using AutoMapper. Ensure AutoMapper is provided.
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="ignoreOtherProperties"></param>
        /// <returns></returns>
        TDestination ResolveAndMap<TDestination>(object source, bool ignoreOtherProperties = false)
            where TDestination : class;
        #endregion Public Methods
    }

    public interface IEntityResolver : IEntityResolver<DbContext>
    {
    }
}