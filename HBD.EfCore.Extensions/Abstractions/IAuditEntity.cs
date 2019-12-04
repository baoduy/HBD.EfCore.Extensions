using HBD.EfCore.Extensions.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HBD.EfCore.Extensions.Abstractions
{
    public interface IAuditEntity<out TKey> : IConcurrencyEntity<TKey>
    {
        #region Properties

        [Required]
        [MaxLength(255)]
        [IgnoreFromUpdate]
        string CreatedBy { get; }

        [Required] [IgnoreFromUpdate] DateTimeOffset CreatedOn { get; }

        [MaxLength(255)] [IgnoreFromUpdate] string UpdatedBy { get; }

        [IgnoreFromUpdate] DateTimeOffset? UpdatedOn { get; }

        #endregion Properties
    }

    public interface IAuditEntity : IAuditEntity<int>, IConcurrencyEntity
    {
    }
}