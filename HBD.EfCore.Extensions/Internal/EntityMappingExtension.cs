using System.Collections.Generic;
using System.Reflection;
using HBD.EfCore.Extensions.Options;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HBD.EfCore.Extensions.Internal
{
    internal class EntityMappingExtension : IDbContextOptionsExtension, IEntityMappingExtension
    {
        #region Public Properties

        public string LogFragment => $"using {nameof(EntityMappingExtension)}";

        #endregion Public Properties

        #region Internal Properties

        internal ICollection<RegistrationInfo> Registrations { get; } = new List<RegistrationInfo>();

        #endregion Internal Properties

        #region Public Methods

        public bool ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(new EntityMappingService(this));

            using (var provider = services.BuildServiceProvider())
            {
                var originalModelCustomizer = provider.GetService<IModelCustomizer>();

                services.Replace(new ServiceDescriptor(
                    typeof(IModelCustomizer),
                    new ExtraModelCustomizer(originalModelCustomizer)
                ));
            }

            return false;
        }

        /// <inheritdoc />
        /// <summary>
        /// The Assemblies will be scan
        /// </summary>
        /// <param name="entityAssemblies"></param>
        /// <returns></returns>
        public RegistrationInfo ScanFrom(params Assembly[] entityAssemblies)
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