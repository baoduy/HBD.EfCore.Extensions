using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HBD.EntityFrameworkCore.Extensions.Options
{
    public class EntityMappingExtension : IDbContextOptionsExtension, IEntityMappingExtension
    {
        internal ICollection<RegistrationInfo> Registrations { get; } = new List<RegistrationInfo>();

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

        public bool ApplyServices(IServiceCollection services) => true;

        public long GetServiceProviderHashCode() => nameof(EntityMappingExtension).GetHashCode();

        public void Validate(IDbContextOptions options)
        {
            foreach (var info in Registrations)
                info.Validate();
        }

        public string LogFragment => nameof(EntityMappingExtension);
    }
}
