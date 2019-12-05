using System;
using HBD.EfCore.Extensions.Abstractions;

namespace HBD.EfCore.DDD.Domains
{
    public interface IDomainEntity : IAuditEntity<Guid>
    {

    }
}