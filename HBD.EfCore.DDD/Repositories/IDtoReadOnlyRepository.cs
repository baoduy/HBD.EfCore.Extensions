using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HBD.EfCore.Extensions.Pageable;
using HBD.EfCore.Extensions.Utilities;

namespace HBD.EfCore.DDD.Repositories
{
    public interface IDtoReadOnlyRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Read List DTO with entity filtering
        /// </summary>    
        public IAsyncEnumerable<TDto> ReadAsync<TDto>(Expression<Func<TEntity, bool>> filter = null, IQueryBuilder<TDto> ordering = null) where TDto : class;

        public IAsyncEnumerable<TDto> ReadIgnoreFiltersAsync<TDto>(Expression<Func<TEntity, bool>> filter = null, IQueryBuilder<TDto> ordering = null) where TDto : class;

        public Task<IPageable<TDto>> ReadPageAsync<TDto>(int pageIndex, int pageSize, IQueryBuilder<TDto> ordering, Expression<Func<TEntity, bool>> filter = null) where TDto : class;

        public Task<IPageable<TDto>> ReadPageIgnoreFiltersAsync<TDto>(int pageIndex, int pageSize, IQueryBuilder<TDto> ordering, Expression<Func<TEntity, bool>> filter = null) where TDto : class;
    }
}