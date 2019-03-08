using System;
using System.Collections.Generic;
using AutoMapper;
using HBD.EfCore.EntityResolver.Attributes;
using HBD.EfCore.EntityResolver.Tests.Entities;
using HBD.EfCore.EntityResolver.Tests.Specs;
using Account = HBD.EfCore.EntityResolver.Tests.Entities.Account;

namespace HBD.EfCore.EntityResolver.Tests.Models
{
    public class UserModel : ILinkToEntity<User>, IOther
    {
        [AutoResolve(typeof(Account))]
        public int AccountId { get; set; }

        [AutoResolve(typeof(Account), SpecType = typeof(AccountSpec))]
        public AccountBasicViewModel OtherAccount { get; set; }

        [AutoResolve()]
        public AccountBasicViewModel OtherAccountWithoutSpec { get; set; }

        [AutoResolve(typeof(Account))]
        public IList<AccountBasicViewModel> ListAccounts { get; set; } = new List<AccountBasicViewModel>();

        [AlwaysIncluded]
        public DateTime CreateDateTime { get; }=DateTime.Now;
    }

    internal sealed class UserModelMapper : Profile
    {
        public UserModelMapper() => CreateMap<UserModel, User>(MemberList.Source);
    }
}
