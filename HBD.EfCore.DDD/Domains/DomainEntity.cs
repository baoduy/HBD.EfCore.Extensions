using System;
using HBD.EfCore.Extensions.Abstractions;

namespace HBD.EfCore.DDD.Domains
{
    /// <summary>
    /// Domain Entity
    /// </summary>
    public abstract class DomainEntity : AuditEntityGuid
    {
        #region Constructors

        protected DomainEntity()
        {
        }

        protected DomainEntity(Guid id) : base(id)
        {
        }

        protected DomainEntity(Guid id, string createdBy, DateTimeOffset? createdOn = null) : base(id, createdBy, createdOn)
        {
        }

        protected DomainEntity(string createdBy, DateTimeOffset? createdOn = null) : base(Guid.NewGuid(), createdBy, createdOn)
        {
        }

        #endregion Constructors
    }
}