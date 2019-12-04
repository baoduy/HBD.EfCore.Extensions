using GenericEventRunner.ForDbContext;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.DDD
{
    public abstract class BoundedContext : DbContextWithEvents
    {
        #region Constructors

        protected BoundedContext(DbContextOptions options, IEventsRunner eventsRunner) : base(options, eventsRunner)
        {
        }

        #endregion Constructors
    }
}