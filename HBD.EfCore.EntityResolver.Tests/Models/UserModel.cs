using System;
using System.Collections.Generic;
using AutoMapper;
using HBD.EfCore.EntityResolver.Tests.Entities;
using HBD.EfCore.EntityResolver.Tests.Specs;
using HBD.EfCore.EntityResolvers.Attributes;
using Account = HBD.EfCore.EntityResolver.Tests.Entities.Account;

namespace HBD.EfCore.EntityResolver.Tests.Models
{
    public class UserModel : ILinkToEntity<User>, IOther
    {
        #region Properties

        [AutoResolve(typeof(Account))]
        public int AccountId { get; set; }

        [AlwaysIncluded]
        public DateTime CreateDateTime { get; } = DateTime.Now;

        [AutoResolve(typeof(Account))]
        public IList<AccountBasicViewModel> ListAccounts { get; set; } = new List<AccountBasicViewModel>();

        [AutoResolve(typeof(Account), SpecType = typeof(AccountSpec))]
        public AccountBasicViewModel OtherAccount { get; set; }

        [AutoResolve()]
        public AccountBasicViewModel OtherAccountWithoutSpec { get; set; }

        #endregion Properties
    }

    internal sealed class UserModelMapper : Profile
    {
        #region Constructors

        public UserModelMapper() => CreateMap<UserModel, User>(MemberList.Source);

        #endregion Constructors
    }
}