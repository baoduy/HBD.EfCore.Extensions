using System.Linq;

namespace HBD.EfCore.Extensions.Utilities
{
    public interface IQueryBuilder<T> where T : class
    {
        #region Methods

        IQueryable<T> Build(IQueryable<T> query);

        #endregion Methods
    }
}