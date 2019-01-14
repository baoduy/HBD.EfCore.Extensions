using System.ComponentModel.DataAnnotations;

namespace HBD.EfCore.Extensions.Abstractions
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