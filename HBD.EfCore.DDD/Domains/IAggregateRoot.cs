using GenericEventRunner.ForEntities;
using HBD.EfCore.Extensions.Abstractions;
using System;
using System.Collections.Generic;

namespace HBD.EfCore.DDD.Domains
{
    public interface IAggregateRoot : IAuditEntity<Guid>
    {
        public void AddEvent(IDomainEvent dEvent, EventToSend eventToSend = EventToSend.BeforeSave);
        public ICollection<IDomainEvent> GetAfterSaveEventsThenClear();
        public ICollection<IDomainEvent> GetBeforeSaveEventsThenClear();
    }
}