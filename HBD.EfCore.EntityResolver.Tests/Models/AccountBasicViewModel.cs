using AutoMapper;
using HBD.EfCore.EntityResolver.Tests.Entities;

namespace HBD.EfCore.EntityResolver.Tests.Models
{
    public sealed class AccountBasicViewModel : ILinkToEntity<Account>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    internal sealed class AccountBasicViewModelMapper : Profile
    {
        public AccountBasicViewModelMapper() => CreateMap<AccountBasicViewModel, Account>(MemberList.Source);
    }
}
