using HBD.EntityFrameworkCore.Extensions.Pageable;
using System;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class PageableExtensions
    {
        private static void Validate(int pageIndex, int pageSize)
        {
            if (pageIndex < 0) throw new ArgumentException($"{nameof(pageIndex)} should be >= 0");
            if (pageSize <= 0) throw new ArgumentException($"{nameof(pageSize)} should be > 0");
        }

        public static IPageable<TEntity> ToPageable<TEntity>(this IOrderedQueryable<TEntity> query, int pageIndex,
            int pageSize)
        {
            Validate(pageIndex, pageSize);

            //Catch to improve the performance
            var totalItems = query.DeferredCount().FutureValue();

            var itemIndex = pageIndex * pageSize;
            if (itemIndex < 0) itemIndex = 0; //Get first Page
            if (itemIndex >= totalItems) itemIndex = totalItems - pageSize; //Get last page.

            var items = pageSize >= totalItems ? query.Future() : query.Skip(itemIndex).Take(pageSize).Future();

            return new Pageable<TEntity>(pageIndex, pageSize, totalItems, items.ToList());
        }

        public static async Task<IPageable<TEntity>> ToPageableAsync<TEntity>(this IOrderedQueryable<TEntity> query,
            int pageIndex, int pageSize)
        {
            Validate(pageIndex, pageSize);

            //Catch to improve the performance
            var totalItems = query.DeferredCount().FutureValue();

            var itemIndex = pageIndex * pageSize;
            if (itemIndex < 0) itemIndex = 0; //Get first Page
            if (itemIndex >= totalItems) itemIndex = totalItems - pageSize; //Get last page.

            var items = pageSize >= totalItems ? query.Future() : query.Skip(itemIndex).Take(pageSize).Future();

            return new Pageable<TEntity>(pageIndex, pageSize, totalItems, await items.ToListAsync());
        }
    }
}
