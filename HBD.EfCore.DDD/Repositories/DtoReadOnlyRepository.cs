using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HBD.EfCore.Extensions.Pageable;
using HBD.EfCore.Extensions.Utilities;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.DDD.Repositories
{
    public class DtoReadOnlyRepository<TEntity> : IDtoReadOnlyRepository<TEntity> where TEntity : class
    {
        private readonly IMapper mapper;
        private readonly DbContext dbContext;

        public DtoReadOnlyRepository(IMapper mapper, DbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        protected virtual IQueryable<TEntity> GetQuery(bool ignoreFilters)
        {
            var query = ignoreFilters ? dbContext.Set<TEntity>().IgnoreQueryFilters() : dbContext.Set<TEntity>();
            return query.ApplyTracking(false);
        }
        protected virtual IQueryable<TDto> GetQuery<TDto>(Expression<Func<TEntity, bool>> filter, IQueryBuilder<TDto> ordering, bool ignoreFilters) where TDto : class
        {
            var query = GetQuery(ignoreFilters);
            if (filter != null)
                query = query.Where(filter);

            if (ordering != null)
                return ordering.Build(query.ProjectTo<TDto>(mapper.ConfigurationProvider));
            return query.ProjectTo<TDto>(mapper.ConfigurationProvider);
        }

        // protected virtual IQueryable<TEntity> GetQuery(Spec<TEntity> spec, IQueryBuilder<TEntity> ordering, bool tracking, bool ignoreFilters)
        // {
        //     var query = GetQuery( ignoreFilters);
        //     return query.ApplySpecAndOrder(spec, ordering);
        // }

        private IQueryable<TDto> GetQuery<TDto>(Expression<Func<TEntity, bool>> filter, Expression<Func<TDto, object>> ordering, bool ignoreFilters)
        {
            var query = GetQuery(ignoreFilters);
            if (filter != null)
                query = query.Where(filter);

            if (ordering == null)
                return query.ProjectTo<TDto>(mapper.ConfigurationProvider);
            return query.ProjectTo<TDto>(mapper.ConfigurationProvider).OrderBy(ordering);
        }

        // private IQueryable<TEntity> GetQuery<TKey>(Spec<TEntity> spec, Expression<Func<TEntity, TKey>> ordering, bool tracking, bool ignoreFilters)
        // {
        //     var query = GetQuery( ignoreFilters);
        //     return query.ApplySpecAndOrder(spec, ordering);
        // }

        public IAsyncEnumerable<TDto> ReadAsync<TDto>(Expression<Func<TEntity, bool>> filter = null, IQueryBuilder<TDto> ordering = null) where TDto : class
            => GetQuery(filter, ordering, false).AsAsyncEnumerable();

        public IAsyncEnumerable<TDto> ReadIgnoreFiltersAsync<TDto>(Expression<Func<TEntity, bool>> filter = null, IQueryBuilder<TDto> ordering = null) where TDto : class
            => GetQuery(filter, ordering, true).AsAsyncEnumerable();

        public Task<IPageable<TDto>> ReadPageAsync<TDto>(int pageIndex, int pageSize, IQueryBuilder<TDto> ordering, Expression<Func<TEntity, bool>> filter = null) where TDto : class
        {
            var query = GetQuery(filter, ordering, false) as IOrderedQueryable<TDto>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        public Task<IPageable<TDto>> ReadPageIgnoreFiltersAsync<TDto>(int pageIndex, int pageSize, IQueryBuilder<TDto> ordering, Expression<Func<TEntity, bool>> filter = null) where TDto : class
        {
            var query = GetQuery(filter, ordering, true) as IOrderedQueryable<TDto>;
            return query.ToPageableAsync(pageIndex, pageSize);
        }

        // public System.Collections.Generic.IAsyncEnumerable<TDto> ReadAsync<TDto>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TDto, object>> ordering = null)
        //     => GetQuery(filter, ordering, false).AsAsyncEnumerable();
    }
}