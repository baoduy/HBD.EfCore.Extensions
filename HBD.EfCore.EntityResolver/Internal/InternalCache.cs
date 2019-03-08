using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HBD.Framework.Extensions;

[assembly: InternalsVisibleTo("HBD.EfCore.Extensions.Tests")]
namespace HBD.EfCore.EntityResolver.Internal
{
    internal static class InternalCache
    {
        #region Private Fields

        private static readonly ConcurrentDictionary<Type, IReadOnlyCollection<ResolverPropertyInfo>> ModelCache =
            new ConcurrentDictionary<Type, IReadOnlyCollection<ResolverPropertyInfo>>();

        #endregion Private Fields

        #region Public Methods

        public static IReadOnlyCollection<ResolverPropertyInfo> GetModelInfo(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return ModelCache.GetOrAdd(model.GetType(), 
                t => Extensions.GetResolveInfo(t).ToReadOnly());
        }

        #endregion Public Methods
    }
}