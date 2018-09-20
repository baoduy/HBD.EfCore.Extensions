using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public abstract class AuditEntity : AuditEntity<long>, IAuditEntity
    {
        /// <inheritdoc />
        protected AuditEntity(string createdBy):base(createdBy)
        {
          
        }

        /// <inheritdoc />
        protected AuditEntity() { }
    }

    public abstract class Entity : Entity<long>, IEntity
    {
        /// <summary>
        /// Constructor for EF Core
        /// </summary>
        protected Entity() { }
    }

    public abstract class AuditEntity<TKey> : Entity<TKey>, IAuditEntity<TKey>
    {
        /// <inheritdoc />
        protected AuditEntity(string createdBy)
        {
            CreatedBy = createdBy ?? throw new ArgumentNullException(nameof(createdBy));
            CreatedOn = DateTimeOffset.Now;
        }

        /// <inheritdoc />
        protected AuditEntity()
        {
        }


        public string CreatedBy { get; private set; }

        public DateTimeOffset CreatedOn { get; private set; }

        public string UpdatedBy { get; private set; }

        public DateTimeOffset? UpdatedOn { get; private set; }

        #region Actions
        /// <summary>
        /// Update Audit info.
        /// </summary>
        /// <param name="userName"></param>
        protected virtual void SetUpdatedBy(string userName)
        {
            this.UpdatedBy = userName ?? throw new ArgumentNullException(nameof(userName));
            this.UpdatedOn = DateTimeOffset.Now;
        }

        #endregion
    }

    public abstract class Entity<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// Constructor for EF Core
        /// </summary>
        protected Entity()
        {
        }


        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; private set; }

        /// <summary>
        /// The ConcurrencyCheck which using by EF
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public byte[] RowVersion { get; private set; }
    }
}
