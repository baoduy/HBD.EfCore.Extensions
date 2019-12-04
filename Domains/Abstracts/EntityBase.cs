using HBD.EfCore.Extensions.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Domains.Abstracts
{
    public enum DeleteType
    {
        SoftDelete = 1,
        PermanentDelete = 2
    }

    public abstract class EntityBase : AuditEntityGuid
    {
        #region Constructors

        protected EntityBase(string createdBy, DateTimeOffset? createdOn = null) : this(Guid.NewGuid(), createdBy, createdOn)
        {
        }

        protected EntityBase(Guid id, string createdBy, DateTimeOffset? createdOn = null) : base(id, createdBy, createdOn)
        {
        }

        protected EntityBase()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///Handling deleting action. if not the entity is always allow to be deleted.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public abstract ValueTask<bool> IsDeletableAsync(DeleteType type, DbContext service);

        #endregion Methods
    }
}