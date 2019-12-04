using HBD.EfCore.Extensions.Options;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HBD.EfCore.Extensions.Internal
{
    internal class EntityMappingExtension : IDbContextOptionsExtension, IEntityMappingExtension
    {
        #region Fields

        private DbContextOptionsExtensionInfo info;

        #endregion Fields

        #region Properties

        public DbContextOptionsExtensionInfo Info => info ??= new EntityMappingExtensionInfo(this);

        internal ICollection<RegistrationInfo> Registrations { get; } = new List<RegistrationInfo>();

        #endregion Properties

        #region Methods

        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(new EntityMappingService(this));

            using var provider = services.BuildServiceProvider();
            var originalModelCustomizer = provider.GetService<IModelCustomizer>();

            services.Replace(new ServiceDescriptor(
                typeof(IModelCustomizer),
                new ExtraModelCustomizer(originalModelCustomizer)
            ));
        }

        /// <inheritdoc />
        /// <summary>
        /// The Assemblies will be scan
        /// </summary>
        /// <param name="assembliesToScans"></param>
        /// <returns></returns>
        public RegistrationInfo ScanFrom(params Assembly[] assembliesToScans)
        {
            if (!assembliesToScans.Any())
                assembliesToScans = new[] { Assembly.GetCallingAssembly() };

            var register = new RegistrationInfo(assembliesToScans);
            Registrations.Add(register);
            return register;
        }

        public void Validate(IDbContextOptions options)
        {
            foreach (var info in Registrations)
                info.Validate();
        }

        #endregion Methods
    }
}