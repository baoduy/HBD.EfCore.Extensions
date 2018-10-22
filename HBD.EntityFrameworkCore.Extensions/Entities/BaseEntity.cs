using System;
using System.Collections.Generic;
using System.Text;
using HBD.EntityFrameworkCore.Extensions.Abstractions;

namespace DataLayer
{
    public abstract class BaseEntity: AuditEntity
    {
        /// <inheritdoc />
        protected BaseEntity(string createdBy) : this(0, createdBy) { }

        /// <inheritdoc />
        protected BaseEntity(long id, string createdBy) : base(id,createdBy)
        {
           
        }

        /// <inheritdoc />
        protected BaseEntity()
        {
        }
    }
}
