using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions.Mappers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HBD.EntityFrameworkCore.Extensions
{
    public class EntityAutoMappingDbExtension : IDbContextOptionsExtension
    {
        internal Assembly[] EntityAssemblies { get; private set; }
        internal Expression<Func<Type, bool>> Predicate { get; private set; }
        internal Type DefaultEntityMapperType { get; private set; } = typeof(EntityMapper<>);

        public EntityAutoMappingDbExtension FromAssemblies(params Assembly[] entityAssemblies)
        {
            this.EntityAssemblies = entityAssemblies;
            return this;
        }

        public EntityAutoMappingDbExtension WithFilter(Expression<Func<Type, bool>> predicate)
        {
            this.Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            return this;
        }

        /// <summary>
        /// The default mapper type of the Entity if the custom mapper is not found.
        /// This Mapper type must be a Generic Type and an Instance of IEntityMapper<TEntity>
        /// </summary>
        /// <returns></returns>
        public EntityAutoMappingDbExtension WithDefaultMapperType(Type entityMapperType)
        {
            this.DefaultEntityMapperType = entityMapperType;
            return this;
        }

        public bool ApplyServices(IServiceCollection services) => true;

        public long GetServiceProviderHashCode() => nameof(EntityAutoMappingDbExtension).GetHashCode();

        public void Validate(IDbContextOptions options)
        {
            if (!this.DefaultEntityMapperType.IsGenericType)
                throw new ArgumentException($"The {nameof(DefaultEntityMapperType)} must be a Generic Type.");

            if (!this.DefaultEntityMapperType.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IEntityMapper<>)))
                throw new ArgumentException($"The {nameof(DefaultEntityMapperType)} must be a instance of IEntityMapper<>.");
        }

        public string LogFragment => nameof(EntityAutoMappingDbExtension);
    }
}
