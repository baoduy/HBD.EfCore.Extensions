using System;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public abstract class AuditEntity<TKey> : Entity<TKey>, IAuditEntity<TKey>
    {
        /// <inheritdoc />
        protected AuditEntity(string createdBy) : this(default(TKey), createdBy) { }

        /// <inheritdoc />
        protected AuditEntity(TKey id, string createdBy) : base(id)
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

    public abstract class AuditEntity : AuditEntity<long>, IAuditEntity
    {
        /// <inheritdoc />
        protected AuditEntity(string createdBy) : base(createdBy)
        {
        }

        protected AuditEntity(long id, string createdBy) : base(id, createdBy)
        {
        }

        /// <inheritdoc />
        protected AuditEntity() { }
    }
}