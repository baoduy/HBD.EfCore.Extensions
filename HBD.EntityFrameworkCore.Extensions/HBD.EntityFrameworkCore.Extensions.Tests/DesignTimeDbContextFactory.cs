using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args) 
            => new MyDbContext(new DbContextOptionsBuilder()
            .UseSqliteMemory()
            .RegisterEntities(op => op.FromAssemblies(typeof(MyDbContext).Assembly))
            .Options);
    }
}
