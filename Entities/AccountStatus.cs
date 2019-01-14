using System.ComponentModel.DataAnnotations;
using HBD.EfCore.Extensions.Abstractions;

namespace DataLayer
{
    public class AccountStatus : Entity
    {
        #region Public Constructors

        public AccountStatus()
        {
        }

        public AccountStatus(int id) : base(id)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        [Required] [MaxLength(100)] public string Name { get; set; }

        #endregion Public Properties
    }
}