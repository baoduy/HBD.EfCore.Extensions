using System;
using System.ComponentModel.DataAnnotations;
using HBD.EntityFrameworkCore.Extensions.Attributes;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public interface IAuditEntity<out TKey> : IConcurrencyEntity<TKey>
    {
        #region Public Properties

        [Required]
        [MaxLength(255)]
        [IgnoreFromUpdate]
        string CreatedBy { get; }

        [Required] [IgnoreFromUpdate] DateTimeOffset CreatedOn { get; }

        [MaxLength(255)] [IgnoreFromUpdate] string UpdatedBy { get; }

        [IgnoreFromUpdate] DateTimeOffset? UpdatedOn { get; }

        #endregion Public Properties
    }

    public interface IAuditEntity : IAuditEntity<int>, IConcurrencyEntity
    {
    }
}