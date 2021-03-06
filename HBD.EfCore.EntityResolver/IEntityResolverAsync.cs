﻿using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HBD.EfCore.EntityResolvers
{
    public interface IEntityResolverAsync<out TDbDbContext> where TDbDbContext : DbContext
    {
        #region Properties

        TDbDbContext DbContext { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Transform and map the result to Destination using AutoMapper. Ensure AutoMapper is provided.
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="ignoreOtherProperties"></param>
        /// <returns></returns>
        Task ResolveAndMapAsync<TSource>(TSource source, object destination, bool ignoreOtherProperties = false)
        where TSource : class;

        /// <summary>
        /// Transform and map the result to Destination using AutoMapper. Ensure AutoMapper is provided.
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="ignoreOtherProperties"></param>
        /// <returns></returns>
        Task<TDestination> ResolveAndMapAsync<TDestination>(object source, bool ignoreOtherProperties = false)
            where TDestination : class;

        /// <summary>
        /// Transform The Model
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">The model will be transformed</param>
        /// <param name="ignoreOtherProperties">If = true the result will content the transforming properties only and all other property</param>
        /// <returns></returns>
        Task<dynamic> ResolveAsync<TSource>(TSource source, bool ignoreOtherProperties = false) where TSource : class;

        #endregion Methods
    }

    public interface IEntityResolverAsync : IEntityResolverAsync<DbContext>
    {
    }
}