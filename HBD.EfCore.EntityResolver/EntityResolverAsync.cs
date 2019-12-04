using AutoMapper;
using HBD.EfCore.EntityResolvers.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace HBD.EfCore.EntityResolvers
{
    public class EntityResolverAsync : EntityResolverAsync<DbContext>, IEntityResolverAsync
    {
        #region Constructors

        public EntityResolverAsync(DbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        #endregion Constructors
    }

    public class EntityResolverAsync<TDbDbContext> : IEntityResolverAsync<TDbDbContext> where TDbDbContext : DbContext
    {
        #region Constructors

        public EntityResolverAsync(TDbDbContext dbContext, IMapper mapper = null)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        #endregion Constructors

        #region Properties

        public TDbDbContext DbContext { get; }

        public IMapper Mapper { get; }

        protected MethodInfo GetEntitiesKeyOrSpecAsyncMethod { get; } = typeof(ResolverExtensions)
                                    .GetMethod(nameof(ResolverExtensions.GetEntitiesByKeyOrSpecAsync), BindingFlags.Static | BindingFlags.NonPublic);

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public async Task ResolveAndMapAsync<TModel>(TModel source, object destination, bool ignoreOtherProperties = false)
            where TModel : class
        {
            EnsureMapperAvailable();
            var results = await ResolveAsync(source, ignoreOtherProperties).ConfigureAwait(false);
            if (results == null) return;
            Mapper.Map(results, destination);
        }

        /// <inheritdoc/>
        public async Task<TDestination> ResolveAndMapAsync<TDestination>(object source, bool ignoreOtherProperties = false)
            where TDestination : class
        {
            EnsureMapperAvailable();
            var results = await ResolveAsync(source, ignoreOtherProperties).ConfigureAwait(false);
            return results == null ? null : (TDestination)Mapper.Map<TDestination>(results);
        }

        /// <inheritdoc/>
        public async Task<dynamic> ResolveAsync<TModel>(TModel model, bool ignoreOtherProperties = false) where TModel : class
        {
            var config = InternalCache.GetModelInfo(model);
            if (!config.Any())
                return null;

            IDictionary<string, object> dynamicModel = ignoreOtherProperties ? new ExpandoObject() : model.ToDynamic();

            foreach (var propertyInfo in config)
            {
                var propValue = propertyInfo.Property.GetValue(model);
                var name = GetPropName(propertyInfo.Property.Name);

                if (propertyInfo.AlwaysIncluded)
                {
                    dynamicModel[name] = propValue;
                    continue;
                }

                if (propValue == null) continue;

                var items = new List<dynamic>();

                await foreach (var i in ResolveItem(propValue, propertyInfo))
                    items.Add(i);

                dynamicModel[name] = propValue is ICollection ? items : items.SingleOrDefault();
            }

            return dynamicModel;
        }

        protected virtual string GetPropName(string originalName)
            => originalName == null
            ? originalName
            : originalName
                .Replace("Id", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("_Id", string.Empty, StringComparison.OrdinalIgnoreCase);

        protected virtual IAsyncEnumerable<dynamic> ResolveItem(object value, ResolverPropertyInfo resolverPropertyInfo)
        {
            if (resolverPropertyInfo is null)
                throw new ArgumentNullException(nameof(resolverPropertyInfo));

            var md = GetEntitiesKeyOrSpecAsyncMethod.MakeGenericMethod(resolverPropertyInfo.Attribute.EntityType);
            var specType = resolverPropertyInfo.Attribute.SpecType;

            return (IAsyncEnumerable<dynamic>)md.Invoke(this, new[] { DbContext, value, specType });
        }

        private void EnsureMapperAvailable()
        {
            if (Mapper == null) throw new InvalidOperationException($"{nameof(Mapper)} is NULL");
        }

        #endregion Methods
    }
}