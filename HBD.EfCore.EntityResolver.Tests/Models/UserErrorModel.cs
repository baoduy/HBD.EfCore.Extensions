using HBD.EfCore.EntityResolver.Attributes;
using HBD.EfCore.EntityResolver.Tests.Entities;

namespace HBD.EfCore.EntityResolver.Tests.Models
{
    public class UserErrorModel : ILinkToEntity<User>, IOther
    {
        //Throw Exception here
        [AutoResolve()]
        public int AccountId { get; set; }
    }
}
