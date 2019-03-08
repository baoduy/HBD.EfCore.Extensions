using System.ComponentModel.DataAnnotations;
using HBD.EfCore.Extensions.Abstractions;

namespace HBD.EfCore.EntityResolver.Tests.Entities
{
    public class Account : AuditEntity
    {
        //public virtual User User { get; set; }

        #region Public Properties

        [Required] public string Password { get; set; }
        [Required] public string UserName { get; set; }

        #endregion Public Properties
    }
}