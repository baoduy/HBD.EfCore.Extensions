using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public interface IConcurrencyEntity<out TKey> : IEntity<TKey>
    {
        #region Public Properties

        [Timestamp] [ConcurrencyCheck] byte[] RowVersion { get; }

        #endregion Public Properties
    }

    public interface IConcurrencyEntity : IConcurrencyEntity<int>, IEntity
    {
    }
}