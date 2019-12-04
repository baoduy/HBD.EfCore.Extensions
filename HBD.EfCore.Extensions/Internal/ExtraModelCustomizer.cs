using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace HBD.EfCore.Extensions.Internal
{
    internal class ExtraModelCustomizer : IModelCustomizer
    {
        #region Fields

        private readonly IModelCustomizer _original;

        #endregion Fields

        #region Constructors

        public ExtraModelCustomizer(IModelCustomizer original) => _original = original;

        #endregion Constructors

        #region Methods

        public void Customize(ModelBuilder modelBuilder, DbContext context)
        {
            ConfigModelCreating(context, modelBuilder);
            _original?.Customize(modelBuilder, context);
        }

        private void ConfigModelCreating(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            var options = dbContext.GetService<EntityMappingService>()?.EntityMapping;
            if (options == null) return;

            if (options.Registrations.Count <= 0)
                options.ScanFrom(dbContext.GetType().Assembly);

            //Register Entities
            modelBuilder.RegisterEntityMappingFrom(options.Registrations);

            //Register StaticData Of
            modelBuilder.RegisterStaticDataFrom(options.Registrations);

            //Register Data Seeding
            modelBuilder.RegisterDataSeedingFrom(options.Registrations);
        }

        #endregion Methods
    }
}