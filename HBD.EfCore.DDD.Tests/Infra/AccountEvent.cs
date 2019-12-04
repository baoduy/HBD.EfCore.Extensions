using AutoMapper;
using GenericEventRunner.ForEntities;
using System;

namespace HBD.EfCore.DDD.Tests.Infra
{
    [AutoMap(typeof(Account))]
    public class AccountEvent : IDomainEvent
    {
        #region Properties

        public Guid Id { get; set; }

        public string Name { get; set; }

        #endregion Properties
    }
}