using HBD.EntityFrameworkCore.Extensions.Abstractions;

namespace DataLayer
{
    public abstract class BaseEntity: AuditEntity<int>
    {
        /// <inheritdoc />
        protected BaseEntity(string createdBy) : this(0, createdBy) { }

        /// <inheritdoc />
        protected BaseEntity(int id, string createdBy) : base(id,createdBy)
        {
           
        }

        /// <inheritdoc />
        protected BaseEntity()
        {
        }
    }
}
