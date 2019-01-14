using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HBD.EfCore.Extensions.Options
{
    public class EntityMappingExtension : IDbContextOptionsExtension, IEntityMappingExtension
    {
        #region Public Properties

        public string LogFragment => nameof(EntityMappingExtension);

        #endregion Public Properties

        #region Internal Properties

        internal ICollection<RegistrationInfo> Registrations { get; } = new List<RegistrationInfo>();

        #endregion Internal Properties

        #region Public Methods

        public bool ApplyServices(IServiceCollection services) => true;

        /// <summary>
        /// The Assemblies will be scan
        /// </summary>
        /// <param name="entityAssemblies"></param>
        /// <returns></returns>
        public RegistrationInfo FromAssemblies(params Assembly[] entityAssemblies)
        {
            var register = new RegistrationInfo(entityAssemblies);
            Registrations.Add(register);
            return register;
        }

        public long GetServiceProviderHashCode() => nameof(EntityMappingExtension).GetHashCode();

        public void Validate(IDbContextOptions options)
        {
            foreach (var info in Registrations)
                info.Validate();
        }

        #endregion Public Methods
    }
}