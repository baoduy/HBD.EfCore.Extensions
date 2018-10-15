using System;
using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public interface IAuditEntity<out TKey> :IConcurrencyEntity<TKey>
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

    public interface IAuditEntity : IAuditEntity<long>, IConcurrencyEntity
    {

    }
}
