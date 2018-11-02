using System;
using System.ComponentModel.DataAnnotations;
using HBD.EntityFrameworkCore.Extensions.Attributes;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public interface IAuditEntity<out TKey> :IConcurrencyEntity<TKey>
    {
        [Required]
        [MaxLength(255)]
        [IgnoreFromUpdate]
        string CreatedBy { get; }

        [Required]
        [IgnoreFromUpdate]
        DateTimeOffset CreatedOn { get; }

        [MaxLength(255)]
        [IgnoreFromUpdate]
        string UpdatedBy { get; }

        [IgnoreFromUpdate]
        DateTimeOffset? UpdatedOn { get; }
    }

    public interface IAuditEntity : IAuditEntity<long>, IConcurrencyEntity
    {

    }
}
