using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HBD.EfCore.Extensions.Abstractions;
using HBD.EfCore.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Extensions
{
    public abstract class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        #region Private Fields

        private EntityMappingExtension _options;

        #endregion Private Fields

        #region Protected Constructors

        protected DbContext()
        {
        }

        protected DbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <inheritdoc />
        /// <summary>
        /// The Default Delete Behavior which applied to the Required Foreign Keys
        /// </summary>
        //public DeleteBehavior? GlobalDeleteBehavior { get; set; } = null;

        #endregion Public Properties

        #region Protected Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            _options = optionsBuilder.Options.FindExtension<EntityMappingExtension>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (_options == null) return;

            //If there is no Assembly provided then scan the DbContext Assembly
            if (_options.Registrations.Count <= 0)
                _options.FromAssemblies(GetType().Assembly);

            //Register Entities
            modelBuilder.RegisterEntityMappingFrom(_options.Registrations);

            //Register StaticData Of
            modelBuilder.RegisterStaticDataFrom(_options.Registrations);

            //Register Data Seeding
            modelBuilder.RegisterDataSeedingFrom(_options.Registrations);

            //Apply Global Delete behavior
            //if (GlobalDeleteBehavior != null)
            //{
            //    //Only applied for Required Foreign Keys only.
            //    foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())
            //                // Only Required Keys, Not Owned Type and Behavior is Cascade
            //                .Where(k => k.IsRequired && !k.IsOwnership && k.DeleteBehavior == DeleteBehavior.Cascade))
            //        relationship.DeleteBehavior = GlobalDeleteBehavior.Value;
            //}
        }

        protected virtual void OnSaving()
        {
            if (!ChangeTracker.HasChanges()) return;

            var entities = ChangeTracker.Entries().Where(e => e.Entity is ISavingAwareness).Select(e => new { Entity = e.Entity as ISavingAwareness, e.State });

            foreach (var entity in entities)
                entity.Entity.OnSaving(entity.State, this);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            OnSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        #endregion Protected Methods
    }
}