using HBD.EfCore.Extensions.Abstractions;

namespace DataLayer
{
    public abstract class BaseEntity : AuditEntity
    {
        #region Protected Constructors

        /// <inheritdoc/>
        protected BaseEntity(string createdBy) : this(0, createdBy)
        {
        }

        /// <inheritdoc/>
        protected BaseEntity(int id, string createdBy) : base(id, createdBy)
        {
        }

        /// <inheritdoc/>
        protected BaseEntity()
        {
        }

        #endregion Protected Constructors
    }
}