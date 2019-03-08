using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HBD.EfCore.Hooks.SavingAwareness
{
    /// <summary>
    /// The Interface will let DbContext execute whenever the entity is being saved to Db. The
    /// purpose of this method is allow you to calculate all Calculated Properties.
    /// </summary>
    public interface ISavingAware
    {
        #region Public Methods

        Task OnSavingAsync(EntityState state, DbContext dbContext);

        #endregion Public Methods
    }
}