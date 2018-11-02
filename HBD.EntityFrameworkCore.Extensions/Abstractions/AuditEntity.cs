using System;
using System.Collections.Generic;

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
        /// Update UpdatedBy UserName info.
        /// </summary>
        /// <param name="userName"></param>
        protected virtual void SetUpdatedBy(string userName)
        {
            //If ID is default means the entity is not saved yet.
            //Only Set the value if Id >=0
            if (EqualityComparer<TKey>.Default.Equals(Id, default(TKey)))
                return;

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