using System.Collections.Generic;

namespace HBD.EntityFrameworkCore.Extensions.Pageable
{
    public interface IPageable
    {
        #region Public Properties

        int PageIndex { get; }
        int PageSize { get; }
        int TotalItems { get; }
        int TotalPage { get; }

        #endregion Public Properties
    }

    public interface IPageable<out TEntity> : IPageable
    {
        #region Public Properties

        IReadOnlyCollection<TEntity> Items { get; }

        #endregion Public Properties
    }
}