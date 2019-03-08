using DataLayer;
using DataLayer.Mappers;
using HBD.EfCore.Extensions.Configurations;
using HBD.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HBD.EfCore.Extensions.Tests
{
    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        #region Public Methods

        public MyDbContext CreateDbContext(string[] args)
            => new MyDbContext(new DbContextOptionsBuilder()
                .UseSqliteMemory()
                .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly)
                    .WithDefaultMappersType(typeof(AuditEntityMapper<>), typeof(BaseEntityMapper<>),
                        typeof(EntityTypeConfiguration<>)))
                .Options);

        #endregion Public Methods
    }
}