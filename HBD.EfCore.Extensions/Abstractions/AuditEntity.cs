using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.EfCore.Extensions.Abstractions
{
    public abstract class AuditEntity<TKey> : Entity<TKey>, IAuditEntity<TKey>
    {
        #region Fields

        [NotMapped]
        private readonly bool _loadedFromDb = false;

        #endregion Fields

        #region Constructors

        /// <inheritdoc/>
        protected AuditEntity(TKey id, string createdBy, DateTimeOffset? createdOn = null) : base(id)
        {
            CreatedBy = createdBy ?? throw new ArgumentNullException(nameof(createdBy));
            CreatedOn = createdOn ?? DateTimeOffset.Now;
        }

        /// <inheritdoc/>
        protected AuditEntity(TKey id) : base(id) { }

        protected AuditEntity() => _loadedFromDb = true;

        #endregion Constructors

        #region Properties

        [MaxLength(256)]
        [Column(Order = 996)]
        public string CreatedBy { get; private set; }

        [Column(Order = 997)]
        public DateTimeOffset CreatedOn { get; private set; }

        [MaxLength(256)]
        [Column(Order = 998)]
        public string UpdatedBy { get; private set; }

        [Column(Order = 999)]
        public DateTimeOffset? UpdatedOn { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Update UpdatedBy UserName info.
        /// </summary>
        /// <param name="userName"></param>
        protected virtual void SetUpdatedBy(string userName)
        {
            //If ID is default means the entity is not saved yet.
            //Only Set the value if Id >=0
            if (!_loadedFromDb)
            {
                CreatedBy = userName ?? throw new ArgumentNullException(nameof(userName));
                CreatedOn = DateTimeOffset.Now;
                return;
            }

            UpdatedBy = userName ?? throw new ArgumentNullException(nameof(userName));
            UpdatedOn = DateTimeOffset.Now;
        }

        #endregion Methods
    }

    public abstract class AuditEntity : AuditEntity<int>, IAuditEntity
    {
        #region Constructors

        protected AuditEntity(int id, string createdBy, DateTimeOffset? createdOn = null) : base(id, createdBy, createdOn)
        {
        }

        /// <inheritdoc/>
        protected AuditEntity(int id) : base(id)
        {
        }

        /// <inheritdoc/>
        protected AuditEntity()
        {
        }

        #endregion Constructors
    }

    public abstract class AuditEntityGuid : AuditEntity<Guid>, IAuditEntity<Guid>
    {
        #region Constructors

        protected AuditEntityGuid(Guid id, string createdBy, DateTimeOffset? createdOn = null)
            : base(id, createdBy, createdOn)
        {
        }

        /// <inheritdoc/>
        protected AuditEntityGuid(Guid id) : base(id)
        {
        }

        /// <inheritdoc/>
        protected AuditEntityGuid() : base(Guid.NewGuid())
        {
        }

        #endregion Constructors
    }
}