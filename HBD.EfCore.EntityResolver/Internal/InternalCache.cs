using HBD.Framework.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HBD.EfCore.Extensions.Tests")]

namespace HBD.EfCore.EntityResolvers.Internal
{
    internal static class InternalCache
    {
        #region Fields

        private static readonly ConcurrentDictionary<Type, IReadOnlyCollection<ResolverPropertyInfo>> ModelCache =
            new ConcurrentDictionary<Type, IReadOnlyCollection<ResolverPropertyInfo>>();

        #endregion Fields

        #region Methods

        public static IReadOnlyCollection<ResolverPropertyInfo> GetModelInfo(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return ModelCache.GetOrAdd(model.GetType(),
                t => ResolverExtensions.GetResolveInfo(t).ToReadOnly());
        }

        #endregion Methods
    }
}