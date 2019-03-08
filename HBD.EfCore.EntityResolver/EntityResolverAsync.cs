using AutoMapper;
using HBD.EfCore.EntityResolver.Internal;
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

namespace HBD.EfCore.EntityResolver
{
    public class EntityResolverAsync : EntityResolverAsync<DbContext>, IEntityResolverAsync
    {
        #region Public Constructors

        public EntityResolverAsync(DbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        #endregion Public Constructors
    }

    public class EntityResolverAsync<TDbDbContext> : IEntityResolverAsync<TDbDbContext> where TDbDbContext : DbContext
    {
        #region Public Constructors

        public EntityResolverAsync(TDbDbContext dbContext, IMapper mapper = null)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        #endregion Public Constructors

        #region Public Properties

        public TDbDbContext DbContext { get; }

        public IMapper Mapper { get; }

        #endregion Public Properties

        #region Protected Properties

        protected MethodInfo GetEntitiesKeyOrSpecAsyncMethod { get; } = typeof(Extensions)
                                    .GetMethod(nameof(Extensions.GetEntitiesByKeyOrSpecAsync), BindingFlags.Static | BindingFlags.NonPublic);

        #endregion Protected Properties

        #region Public Methods

        /// <inheritdoc/>
        public async Task ResolveAndMapAsync<TModel>(TModel source, object destination, bool ignoreOtherProperties = false)
            where TModel : class
        {
            EnsureMapperAvailable();
            var results = await ResolveAsync(source, ignoreOtherProperties);
            if (results == null) return;
            Mapper.Map(results, destination);
        }

        /// <inheritdoc/>
        public async Task<TDestination> ResolveAndMapAsync<TDestination>(object source, bool ignoreOtherProperties = false)
            where TDestination : class
        {
            EnsureMapperAvailable();
            var results = await ResolveAsync(source, ignoreOtherProperties);
            return results == null ? null : (TDestination)Mapper.Map<TDestination>(results);
        }

        /// <inheritdoc/>
        public async Task<dynamic> ResolveAsync<TModel>(TModel model, bool ignoreOtherProperties = false) where TModel : class
        {
            var config = InternalCache.GetModelInfo(model);
            if (!EnumerableExtensions.Any(config))
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
                var items = await ResolveItem(propValue, propertyInfo);

                dynamicModel[name] = propValue is ICollection ? items.ToList() : items.SingleOrDefault();
            }

            return dynamicModel;
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual string GetPropName(string originalName)
            => originalName.Replace("Id", string.Empty)
                .Replace("_Id", string.Empty);

        protected virtual async Task<IEnumerable<dynamic>> ResolveItem(
            object value,
            ResolverPropertyInfo resolverPropertyInfo)
        {
            var md = GetEntitiesKeyOrSpecAsyncMethod.MakeGenericMethod(resolverPropertyInfo.Attribute.EntityType);
            var specType = resolverPropertyInfo.Attribute.SpecType;

            return await (Task<IEnumerable<dynamic>>)md.Invoke(this, new[] { DbContext, value, specType });
        }

        #endregion Protected Methods

        #region Private Methods

        private void EnsureMapperAvailable()
        {
            if (Mapper == null) throw new InvalidOperationException($"{nameof(Mapper)} is NULL");
        }

        #endregion Private Methods
    }
}