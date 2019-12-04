using HBD.EfCore.EntityResolver.Tests.Entities;
using HBD.EfCore.EntityResolvers.Attributes;

namespace HBD.EfCore.EntityResolver.Tests.Models
{
    public class UserErrorModel : ILinkToEntity<User>, IOther
    {
        #region Properties

        //Throw Exception here
        [AutoResolve()]
        public int AccountId { get; set; }

        #endregion Properties
    }
}