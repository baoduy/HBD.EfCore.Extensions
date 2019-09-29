using System;
using HBD.EfCore.Extensions.Abstractions;

namespace DataLayer
{
    public class GuidEntity : EntityGuid
    {
        public string Name { get; set; }
    }

    public class GuidAuditEntity : AuditEntityGuid
    {
        public string Name { get; set; }
    }
}
