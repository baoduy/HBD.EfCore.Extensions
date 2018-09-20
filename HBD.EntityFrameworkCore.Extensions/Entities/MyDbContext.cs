using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class MyDbContext : HBD.EntityFrameworkCore.Extensions.DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
