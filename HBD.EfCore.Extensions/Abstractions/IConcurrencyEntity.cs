using System.ComponentModel.DataAnnotations;

namespace HBD.EfCore.Extensions.Abstractions
{
    public interface IConcurrencyEntity<out TKey> : IEntity<TKey>
    {
        #region Properties

        [Timestamp]
        [ConcurrencyCheck]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>")]
        byte[] RowVersion { get; }

        #endregion Properties
    }

    public interface IConcurrencyEntity : IConcurrencyEntity<int>, IEntity
    {
    }
}