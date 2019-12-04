using AutoMapper;
using HBD.EfCore.EntityResolvers.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace HBD.EfCore.EntityResolvers
{
    public class EntityResolver : EntityResolver<DbContext>, IEntityResolver
    {
        #region Constructors

        public EntityResolver(DbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        #endregion Constructors
    }

    public class EntityResolver<TDbDbContext> : IEntityResolver<TDbDbContext> where TDbDbContext : DbContext
    {
        #region Constructors

        public EntityResolver(TDbDbContext dbContext, IMapper mapper = null)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        #endregion Constructors

        #region Properties

        public TDbDbContext DbContext { get; }

        public IMapper Mapper { get; }

        protected MethodInfo GetEntitiesKeyOrSpecMethod { get; } = typeof(ResolverExtensions)
                                    .GetMethod(nameof(ResolverExtensions.GetEntitiesByKeyOrSpec), BindingFlags.Static | BindingFlags.NonPublic);

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public dynamic Resolve<TModel>(TModel model, bool ignoreOtherProperties = false) where TModel : class
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
                var items = ResolveItem(propValue, propertyInfo);
                dynamicModel[name] = propValue is ICollection ? items.ToList() : items.SingleOrDefault();
            }

            return dynamicModel;
        }

        /// <inheritdoc/>
        public void ResolveAndMap<TModel>(TModel source, object destination, bool ignoreOtherProperties = false)
            where TModel : class
        {
            EnsureMapperAvailable();
            var results = Resolve(source, ignoreOtherProperties);
            if (results == null) return;
            Mapper.Map(results, destination);
        }

        /// <inheritdoc/>
        public TDestination ResolveAndMap<TDestination>(object source, bool ignoreOtherProperties = false)
            where TDestination : class
        {
            EnsureMapperAvailable();
            var results = Resolve(source, ignoreOtherProperties);
            return results == null ? null : (TDestination)Mapper.Map<TDestination>(results);
        }

        protected virtual string GetPropName(string originalName)
            => originalName == null ? originalName : originalName.Replace("Id", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("_Id", string.Empty, StringComparison.OrdinalIgnoreCase);

        protected virtual IEnumerable<dynamic> ResolveItem(
            object value,
            ResolverPropertyInfo resolverPropertyInfo)
        {
            if (resolverPropertyInfo is null)
                throw new ArgumentNullException(nameof(resolverPropertyInfo));

            var md = GetEntitiesKeyOrSpecMethod.MakeGenericMethod(resolverPropertyInfo.Attribute.EntityType);
            var specType = resolverPropertyInfo.Attribute.SpecType;

            return (IEnumerable<dynamic>)md.Invoke(this, new[] { DbContext, value, specType });
        }

        private void EnsureMapperAvailable()
        {
            if (Mapper == null) throw new InvalidOperationException($"{nameof(Mapper)} is NULL");
        }

        #endregion Methods
    }
}