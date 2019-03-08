using System;
using System.ComponentModel.DataAnnotations;

namespace HBD.EfCore.Extensions.Abstractions
{
    public abstract class AuditEntity<TKey> : Entity<TKey>, IAuditEntity<TKey>
    {
        #region Protected Constructors

        /// <inheritdoc/>
        protected AuditEntity(string createdBy) : this(default, createdBy)
        {
        }

        /// <inheritdoc/>
        protected AuditEntity(TKey id, string createdBy) : base(id) 
            => CreatedBy = createdBy ?? throw new ArgumentNullException(nameof(createdBy));

        /// <inheritdoc/>
        protected AuditEntity(TKey id) : base(id)
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        [MaxLength(256)]
        public string CreatedBy { get; private set; }

        public DateTimeOffset CreatedOn { get; private set; }=DateTimeOffset.Now;

        [MaxLength(256)]
        public string UpdatedBy { get; private set; }

        public DateTimeOffset? UpdatedOn { get; private set; }

        #endregion Public Properties

        #region Protected Methods

        /// <summary>
        /// Update UpdatedBy UserName info.
        /// </summary>
        /// <param name="userName"></param>
        protected virtual void SetUpdatedBy(string userName)
        {
            //If ID is default means the entity is not saved yet.
            //Only Set the value if Id >=0
            if (KeyComparer.Equals(Id, default))
            {
                CreatedBy = userName ?? throw new ArgumentNullException(nameof(userName));
                CreatedOn = DateTimeOffset.Now;
                return;
            }

            UpdatedBy = userName ?? throw new ArgumentNullException(nameof(userName));
            UpdatedOn = DateTimeOffset.Now;
        }

        #endregion Protected Methods
    }

    public abstract class AuditEntity : AuditEntity<int>, IAuditEntity
    {
        #region Protected Constructors

        /// <inheritdoc/>
        protected AuditEntity(string createdBy) : base(createdBy)
        {
        }

        protected AuditEntity(int id, string createdBy) : base(id, createdBy)
        {
        }

        /// <inheritdoc/>
        protected AuditEntity() : base(0)
        {
        }

        #endregion Protected Constructors
    }
}