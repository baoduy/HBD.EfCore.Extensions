using System.Linq;

namespace HBD.EfCore.Extensions.OrderBuilders
{
    public interface IQueryBuilder<T> where T : class
    {
        IQueryable<T> Build(IQueryable<T> query);
    }
}