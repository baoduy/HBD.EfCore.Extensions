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
    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        #region Fields

        private readonly DbContext dbContext;

        #endregion Fields

        #region Constructors

        public ReadOnlyRepository(DbContext dbContext) => this.dbContext = dbContext;

        #endregion Constructors

        #region Properties

        protected virtual IQueryable<TEntity> Query => dbContext.Set<TEntity>();

        #endregion Properties

        #region Methods

        public virtual IAsyncEnumerable<TEntity> ReadAsync(Expression<Func<TEntity, bool>> filter = null, IQueryBuilder<TEntity> ordering = null, bool tracking = false)
            => GetQuery(filter, ordering, tracking, false).AsAsyncEnumerable();

        public IAsyncEnumerable<TEntity> ReadAsync<TKey>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TKey>> ordering = null, bool tracking = false)
            => GetQuery(filter, ordering, tracking, false).AsAsyncEnumerable();

        public virtual IAsyncEnumerable<TEntity> ReadIgnoreFildersAsync(Expression<Func<TEntity, bool>> filter = null, IQueryBuilder<TEntity> ordering = null, bool tracking = false)
            => GetQuery(filter, ordering, tracking, true).AsAsyncEnumerable();

        public IAsyncEnumerable<TEntity> ReadIgnoreFildersAsync<TKey>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TKey>> ordering = null, bool tracking = false)
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

        public Task<IPageable<TEntity>> ReadPageIgnoreFildersAsync(int pageIndex, int pageSize, IQueryBuilder<TEntity> ordering, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = GetQuery(filter, ordering, false, true) as IOrderedQueryable<TEntity>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        public Task<IPageable<TEntity>> ReadPageIgnoreFildersAsync(int pageIndex, int pageSize, IQueryBuilder<TEntity> ordering, Spec<TEntity> spec)
        {
            var query = GetQuery(spec, ordering, false, true) as IOrderedQueryable<TEntity>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        public Task<IPageable<TEntity>> ReadPageIgnoreFildersAsync<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> ordering, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = GetQuery(filter, ordering, false, true) as IOrderedQueryable<TEntity>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        public Task<IPageable<TEntity>> ReadPageIgnoreFildersAsync<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> ordering, Spec<TEntity> spec)
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

        public IAsyncEnumerable<TEntity> ReadSpecIgnoreFildersAsync(Spec<TEntity> spec, IQueryBuilder<TEntity> ordering = null, bool tracking = false)
            => GetQuery(spec, ordering, tracking, true).AsAsyncEnumerable();

        public IAsyncEnumerable<TEntity> ReadSpecIgnoreFildersAsync<TKey>(Spec<TEntity> spec, Expression<Func<TEntity, TKey>> ordering = null, bool tracking = false)
           => GetQuery(spec, ordering, tracking, true).AsAsyncEnumerable();

        private IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter, IQueryBuilder<TEntity> ordering, bool tracking, bool ignoreFilters)
        {
            var query = ignoreFilters ? Query.IgnoreQueryFilters() : Query;
            return query
                  .ApplyTracking(tracking)
                  .ApplyFilterAndOrder(filter, ordering);
        }

        private IQueryable<TEntity> GetQuery(Spec<TEntity> spec, IQueryBuilder<TEntity> ordering, bool tracking, bool ignoreFilters)
        {
            var query = ignoreFilters ? Query.IgnoreQueryFilters() : Query;
            return query
               .ApplyTracking(tracking)
               .ApplySpecAndOrder(spec, ordering);
        }

        private IQueryable<TEntity> GetQuery<TKey>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> ordering, bool tracking, bool ignoreFilters)
        {
            var query = ignoreFilters ? Query.IgnoreQueryFilters() : Query;
            return query
                  .ApplyTracking(tracking)
                  .ApplyFilterAndOrder(filter, ordering);
        }

        private IQueryable<TEntity> GetQuery<TKey>(Spec<TEntity> spec, Expression<Func<TEntity, TKey>> ordering, bool tracking, bool ignoreFilters)
        {
            var query = ignoreFilters ? Query.IgnoreQueryFilters() : Query;
            return query
               .ApplyTracking(tracking)
               .ApplySpecAndOrder(spec, ordering);
        }

        #endregion Methods
    }
}