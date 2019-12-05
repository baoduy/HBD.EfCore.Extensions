using System;
using AutoMapper;

namespace HBD.EfCore.DDD.Tests.Infra
{
    [AutoMap(typeof(Infra.Profile))]
    public class ProfileViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}