using HBD.EfCore.Hooks.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks.Tests
{
    public class TestHookDbContext : HookDbContext
    {
        #region Public Constructors

        public TestHookDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Payment>();
        }

        #endregion Protected Methods
    }
}