using System.Collections.Generic;

namespace HBD.EfCore.Extensions.Pageable
{
    public interface IPageable
    {
        #region Properties

        int PageIndex { get; }

        int PageSize { get; }

        int TotalItems { get; }

        int TotalPage { get; }

        #endregion Properties
    }

    public interface IPageable<out TEntity> : IPageable
    {
        #region Properties

        IReadOnlyCollection<TEntity> Items { get; }

        #endregion Properties
    }
}