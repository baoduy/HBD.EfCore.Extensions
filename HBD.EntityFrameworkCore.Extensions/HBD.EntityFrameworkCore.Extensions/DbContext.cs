using HBD.EntityFrameworkCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore;

namespace HBD.EntityFrameworkCore.Extensions
{
    public abstract class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private EntityAutoMappingDbExtension _options = null;

        protected DbContext()
        {
        }

        protected DbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            _options = optionsBuilder.Options.FindExtension<EntityAutoMappingDbExtension>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
            if (_options == null) return;

            //If there is no Assembly provided then scan the DbContext Assembly
            if (_options.EntityAssemblies == null)
                _options.FromAssemblies(this.GetType().Assembly);

            //Register
            modelBuilder.RegisterMappingFromExtension(_options);
        }
    }
}
