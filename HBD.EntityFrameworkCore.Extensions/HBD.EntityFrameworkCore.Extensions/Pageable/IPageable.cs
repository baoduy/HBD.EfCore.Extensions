using System.Collections.Generic;

namespace HBD.EntityFrameworkCore.Extensions.Pageable
{
    public interface IPageable
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalItems { get; }
        int TotalPage { get; }
    }

    public interface IPageable<out TEntity> : IPageable
    {
        IReadOnlyCollection<TEntity> Items { get; }
    }
}
