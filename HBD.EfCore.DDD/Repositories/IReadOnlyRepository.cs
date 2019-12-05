using HBD.EfCore.DDD.Domains;
using HBD.EfCore.Extensions.Pageable;
using HBD.EfCore.Extensions.Specification;
using HBD.EfCore.Extensions.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HBD.EfCore.DDD.Repositories
{
    public interface IReadOnlyRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        #region Methods

        public IAsyncEnumerable<TEntity> ReadAsync(Expression<Func<TEntity, bool>> filter = null, IQueryBuilder<TEntity> ordering = null, bool tracking = false);

        public IAsyncEnumerable<TEntity> ReadAsync<TKey>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TKey>> ordering = null, bool tracking = false);

        /// <summary>
        /// This will Ignore Query Filters from Query
        /// </summary>
        /// <returns></returns>
        public IAsyncEnumerable<TEntity> ReadIgnoreFiltersAsync(Expression<Func<TEntity, bool>> filter = null, IQueryBuilder<TEntity> ordering = null, bool tracking = false);

        /// <summary>
        /// This will Ignore Query Filters from Query
        /// </summary>
        /// <returns></returns>
        public IAsyncEnumerable<TEntity> ReadIgnoreFiltersAsync<TKey>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TKey>> ordering = null, bool tracking = false);

        public Task<IPageable<TEntity>> ReadPageAsync(int pageIndex, int pageSize, IQueryBuilder<TEntity> ordering, Expression<Func<TEntity, bool>> filter = null);

        public Task<IPageable<TEntity>> ReadPageAsync(int pageIndex, int pageSize, IQueryBuilder<TEntity> ordering, Spec<TEntity> spec);

        public Task<IPageable<TEntity>> ReadPageAsync<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> ordering, Expression<Func<TEntity, bool>> filter = null);

        public Task<IPageable<TEntity>> ReadPageAsync<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> ordering, Spec<TEntity> spec);

        /// <summary>
        /// This will Ignore Query Filters from Query
        /// </summary>
        /// <returns></returns>
        public Task<IPageable<TEntity>> ReadPageIgnoreFiltersAsync(int pageIndex, int pageSize, IQueryBuilder<TEntity> ordering, Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// This will Ignore Query Filters from Query
        /// </summary>
        /// <returns></returns>
        public Task<IPageable<TEntity>> ReadPageIgnoreFiltersAsync(int pageIndex, int pageSize, IQueryBuilder<TEntity> ordering, Spec<TEntity> spec);

        /// <summary>
        /// This will Ignore Query Filters from Query
        /// </summary>
        /// <returns></returns>
        public Task<IPageable<TEntity>> ReadPageIgnoreFiltersAsync<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> ordering, Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// This will Ignore Query Filters from Query
        /// </summary>
        /// <returns></returns>
        public Task<IPageable<TEntity>> ReadPageIgnoreFiltersAsync<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> ordering, Spec<TEntity> spec);

        public ValueTask<TEntity> ReadSingleAsync(params object[] id);

        public IAsyncEnumerable<TEntity> ReadSpecAsync(Spec<TEntity> spec, IQueryBuilder<TEntity> ordering = null, bool tracking = false);

        public IAsyncEnumerable<TEntity> ReadSpecAsync<TKey>(Spec<TEntity> spec, Expression<Func<TEntity, TKey>> ordering = null, bool tracking = false);

        /// <summary>
        /// This will Ignore Query Filters from Query
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="ordering"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        public IAsyncEnumerable<TEntity> ReadSpecIgnoreFiltersAsync(Spec<TEntity> spec, IQueryBuilder<TEntity> ordering = null, bool tracking = false);

        /// <summary>
        /// This will Ignore Query Filters from Query
        /// </summary>
        /// <returns></returns>
        public IAsyncEnumerable<TEntity> ReadSpecIgnoreFiltersAsync<TKey>(Spec<TEntity> spec, Expression<Func<TEntity, TKey>> ordering = null, bool tracking = false);

        #endregion Methods
    }
}