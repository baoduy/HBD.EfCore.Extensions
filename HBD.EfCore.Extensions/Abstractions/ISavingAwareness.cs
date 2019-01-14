using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Extensions.Abstractions
{
    /// <summary>
    /// The Interface will let DbContext execute whenever the entity is being saved to Db.
    /// The purpose of this method is allow you to calculate all Calculated Properties.
    /// </summary>
    public interface ISavingAwareness
    {
        void OnSaving(EntityState state, Microsoft.EntityFrameworkCore.DbContext dbContext);
    }
}
