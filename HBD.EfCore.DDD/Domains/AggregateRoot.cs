using GenericEventRunner.ForEntities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.EfCore.DDD.Domains
{

    public abstract class AggregateRoot : EntityEvents, IAggregateRoot
    {
        #region Fields

        [NotMapped]
        private readonly bool _loadedFromDb = false;

        #endregion Fields

        #region Constructors

        protected AggregateRoot(string createdBy, DateTimeOffset? createdOn = null) : this(Guid.NewGuid(), createdBy, createdOn)
        {
        }

        protected AggregateRoot(Guid id, string createdBy, DateTimeOffset? createdOn = null) : this(id)
        {
            CreatedBy = createdBy;
            CreatedOn = createdOn ?? DateTimeOffset.Now;
        }

        /// <inheritdoc/>
        protected AggregateRoot(Guid id) => Id = id;

        /// <inheritdoc/>
        protected AggregateRoot() { _loadedFromDb = true; }

        #endregion Constructors

        #region Properties

        public string CreatedBy { get; private set; }

        public DateTimeOffset CreatedOn { get; private set; }

        public Guid Id { get; private set; }

        public byte[] RowVersion { get; private set; }

        public string UpdatedBy { get; private set; }

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
}