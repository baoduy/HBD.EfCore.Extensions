using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace HBD.EntityFrameworkCore.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Check whether the entity is referring by others.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool IsEntityInUse<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            var entry = context.Entry(entity);

            return entry.Navigations.Any(navigation =>
                !navigation.Metadata.IsCollection()
                && navigation.Query().Any());
        }
    }
}
