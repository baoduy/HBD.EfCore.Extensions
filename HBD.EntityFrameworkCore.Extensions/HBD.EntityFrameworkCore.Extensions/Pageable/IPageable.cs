using System.Collections;
using System.Collections.Generic;

namespace HBD.EntityFrameworkCore.Extensions.Pageable
{
    public interface IPageable : IEnumerable
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalItems { get; }
        int TotalPage { get; }
    }

    public interface IPageable<out TEntity> : IPageable, IEnumerable<TEntity>
    {
        IReadOnlyCollection<TEntity> Items { get; }
    }
}
