using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public interface IConcurrencyEntity<out TKey> : IEntity<TKey>
    {
        [Timestamp]
        [ConcurrencyCheck]
        byte[] RowVersion { get; }
    }

    public interface IConcurrencyEntity : IConcurrencyEntity<long>, IEntity
    {
    }
}
