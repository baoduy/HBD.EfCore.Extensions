using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HBD.EntityFrameworkCore.Extensions.Options
{
    public sealed class RegistrationInfo
    {
        public Assembly[] EntityAssemblies { get; }
        internal Expression<Func<Type, bool>> Predicate { get; private set; }
        internal Type DefaultEntityMapperType { get; private set; } = typeof(EntityTypeConfiguration<>);

        internal RegistrationInfo(params Assembly[] entityAssemblies)
        {
            if (entityAssemblies.Length <= 0)
                throw new ArgumentNullException(nameof(entityAssemblies));

            EntityAssemblies = entityAssemblies;
        }

        public RegistrationInfo WithFilter(Expression<Func<Type, bool>> predicate)
        {
            this.Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            return this;
        }

        /// <summary>
        /// The default mapper type of the Entity if the custom mapper is not found.
        /// This Mapper type must be a Generic Type and an Instance of IEntityMapper<TEntity>
        /// </summary>
        /// <returns></returns>
        public RegistrationInfo WithDefaultMapperType(Type entityMapperType)
        {
            this.DefaultEntityMapperType = entityMapperType;
            return this;
        }

        internal void Validate()
        {
            if (!this.DefaultEntityMapperType.IsGenericType)
                throw new ArgumentException($"The {nameof(DefaultEntityMapperType)} must be a Generic Type.");

            if (!this.DefaultEntityMapperType.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                throw new ArgumentException($"The {nameof(DefaultEntityMapperType)} must be a instance of IEntityMapper<>.");
        }
    }
}
