using HBD.EfCore.DDD.Domains;
using HBD.EfCore.Extensions.Pageable;
using HBD.EfCore.Extensions.Specification;
using HBD.EfCore.Extensions.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HBD.EfCore.DDD.Repositories
{
    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        #region Fields

        private readonly DbContext dbContext;

        #endregion Fields

        #region Constructors

        public ReadOnlyRepository(DbContext dbContext) => this.dbContext = dbContext;

        #endregion Constructors


        #region Methods

        public virtual IAsyncEnumerable<TEntity> ReadAsync(Expression<Func<TEntity, bool>> filter = null, IQueryBuilder<TEntity> ordering = null, bool tracking = false)
            => GetQuery(filter, ordering, tracking, false).AsAsyncEnumerable();

        public IAsyncEnumerable<TEntity> ReadAsync<TKey>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TKey>> ordering = null, bool tracking = false)
            => GetQuery(filter, ordering, tracking, false).AsAsyncEnumerable();

        public virtual IAsyncEnumerable<TEntity> ReadIgnoreFiltersAsync(Expression<Func<TEntity, bool>> filter = null, IQueryBuilder<TEntity> ordering = null, bool tracking = false)
            => GetQuery(filter, ordering, tracking, true).AsAsyncEnumerable();

        public IAsyncEnumerable<TEntity> ReadIgnoreFiltersAsync<TKey>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TKey>> ordering = null, bool tracking = false)
            => GetQuery(filter, ordering, tracking, true).AsAsyncEnumerable();

        public Task<IPageable<TEntity>> ReadPageAsync(int pageIndex, int pageSize, IQueryBuilder<TEntity> ordering, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = GetQuery(filter, ordering, false, false) as IOrderedQueryable<TEntity>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        public Task<IPageable<TEntity>> ReadPageAsync(int pageIndex, int pageSize, IQueryBuilder<TEntity> ordering, Spec<TEntity> spec)
        {
            var query = GetQuery(spec, ordering, false, false) as IOrderedQueryable<TEntity>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        public Task<IPageable<TEntity>> ReadPageAsync<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> ordering, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = GetQuery(filter, ordering, false, false) as IOrderedQueryable<TEntity>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        public Task<IPageable<TEntity>> ReadPageAsync<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> ordering, Spec<TEntity> spec)
        {
            var query = GetQuery(spec, ordering, false, false) as IOrderedQueryable<TEntity>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        public Task<IPageable<TEntity>> ReadPageIgnoreFiltersAsync(int pageIndex, int pageSize, IQueryBuilder<TEntity> ordering, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = GetQuery(filter, ordering, false, true) as IOrderedQueryable<TEntity>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        public Task<IPageable<TEntity>> ReadPageIgnoreFiltersAsync(int pageIndex, int pageSize, IQueryBuilder<TEntity> ordering, Spec<TEntity> spec)
        {
            var query = GetQuery(spec, ordering, false, true) as IOrderedQueryable<TEntity>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        public Task<IPageable<TEntity>> ReadPageIgnoreFiltersAsync<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> ordering, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = GetQuery(filter, ordering, false, true) as IOrderedQueryable<TEntity>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        public Task<IPageable<TEntity>> ReadPageIgnoreFiltersAsync<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> ordering, Spec<TEntity> spec)
        {
            var query = GetQuery(spec, ordering, false, true) as IOrderedQueryable<TEntity>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        public virtual ValueTask<TEntity> ReadSingleAsync(params object[] id)
            => dbContext.Set<TEntity>().FindAsync(id);

        public IAsyncEnumerable<TEntity> ReadSpecAsync(Spec<TEntity> spec, IQueryBuilder<TEntity> ordering = null, bool tracking = false)
            => GetQuery(spec, ordering, tracking, false).AsAsyncEnumerable();

        public IAsyncEnumerable<TEntity> ReadSpecAsync<TKey>(Spec<TEntity> spec, Expression<Func<TEntity, TKey>> ordering = null, bool tracking = false)
            => GetQuery(spec, ordering, tracking, false).AsAsyncEnumerable();

        public IAsyncEnumerable<TEntity> ReadSpecIgnoreFiltersAsync(Spec<TEntity> spec, IQueryBuilder<TEntity> ordering = null, bool tracking = false)
            => GetQuery(spec, ordering, tracking, true).AsAsyncEnumerable();

        public IAsyncEnumerable<TEntity> ReadSpecIgnoreFiltersAsync<TKey>(Spec<TEntity> spec, Expression<Func<TEntity, TKey>> ordering = null, bool tracking = false)
           => GetQuery(spec, ordering, tracking, true).AsAsyncEnumerable();

        protected virtual IQueryable<TEntity> GetQuery(bool tracking, bool ignoreFilters)
        {
            var query = ignoreFilters ? dbContext.Set<TEntity>().IgnoreQueryFilters() : dbContext.Set<TEntity>();
            return query.ApplyTracking(tracking);
        }
        protected virtual IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter, IQueryBuilder<TEntity> ordering, bool tracking, bool ignoreFilters)
        {
            var query = GetQuery(tracking, ignoreFilters);
            return query.ApplyFilterAndOrder(filter, ordering);
        }

        protected virtual IQueryable<TEntity> GetQuery(Spec<TEntity> spec, IQueryBuilder<TEntity> ordering, bool tracking, bool ignoreFilters)
        {
            var query = GetQuery(tracking, ignoreFilters);
            return query.ApplySpecAndOrder(spec, ordering);
        }

        private IQueryable<TEntity> GetQuery<TKey>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> ordering, bool tracking, bool ignoreFilters)
        {
            var query = GetQuery(tracking, ignoreFilters);
            return query.ApplyFilterAndOrder(filter, ordering);
        }

        private IQueryable<TEntity> GetQuery<TKey>(Spec<TEntity> spec, Expression<Func<TEntity, TKey>> ordering, bool tracking, bool ignoreFilters)
        {
            var query = GetQuery(tracking, ignoreFilters);
            return query.ApplySpecAndOrder(spec, ordering);
        }

        #endregion Methods
    }
}