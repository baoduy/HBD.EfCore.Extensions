using System;
using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public interface IAuditEntity : IAuditEntity<long>, IEntity
    {

    }

    public interface IEntity : IEntity<long>
    {

    }

    public interface IAuditEntity<out TKey> : IEntity<TKey>
    {
        [Required]
        [MaxLength(255)]
        string CreatedBy { get; }

        [Required]
        DateTimeOffset CreatedOn { get; }

        [MaxLength(255)]
        string UpdatedBy { get; }

        DateTimeOffset? UpdatedOn { get; }
    }


    public interface IEntity<out TKey>
    {
        [Key]
        TKey Id { get; }

        [Timestamp]
        [ConcurrencyCheck]
        byte[] RowVersion { get; }
    }
}
