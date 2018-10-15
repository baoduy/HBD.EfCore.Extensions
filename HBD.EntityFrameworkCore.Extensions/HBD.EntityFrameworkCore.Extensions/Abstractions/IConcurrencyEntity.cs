using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
