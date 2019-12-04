using GenericEventRunner.ForDbContext;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.DDD.Tests.Infra
{
    internal class ProfileContext : BoundedContext
    {
        #region Constructors

        public ProfileContext(DbContextOptions<ProfileContext> options, IEventsRunner eventsRunner) : base(options, eventsRunner)
        {
        }

        #endregion Constructors
    }
}