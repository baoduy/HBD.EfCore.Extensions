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
        internal Type[] DefaultEntityMapperTypes { get; private set; }

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
        /// This Mapper type must be a Generic Type and an Instance of IEntityTypeConfiguration <see cref="IEntityTypeConfiguration{TEntity}"/>
        /// You can provide the list of Default Mappers which will pickup by its order based on generic argument conditions
        /// </summary>
        /// <returns></returns>
        public RegistrationInfo WithDefaultMappersType(params Type[] entityMapperTypes)
        {
            this.DefaultEntityMapperTypes = entityMapperTypes;
            return this;
        }

        internal void Validate()
        {
            if(DefaultEntityMapperTypes==null)return;

            if (!this.DefaultEntityMapperTypes.All(t => t.IsGenericType))
                throw new ArgumentException($"The {nameof(DefaultEntityMapperTypes)} must be a Generic Type.");

            if (!this.DefaultEntityMapperTypes.All(t => t.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))))
                throw new ArgumentException($"The {nameof(DefaultEntityMapperTypes)} must be a instance of {typeof(IEntityTypeConfiguration<>).Name}.");
        }
    }
}
