using Domains.Abstracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Domains.Profiles
{
    public enum EmployeeType
    {
        Director = 1,
        Secretary = 2,
        Other = 3,
    }

    [Table("Employees", Schema = DomainSchemas.Profile)]
    public class Employee : EntityBase
    {
        #region Constructors

        public Employee(Guid profileId, EmployeeType type, string userName) : base(userName)
        {
            ProfileId = profileId;
            PromoteTo(type, userName);
        }

        private Employee()
        {
        }

        #endregion Constructors

        #region Properties

        public virtual Profile Profile { get; private set; }

        public Guid ProfileId { get; private set; }

        public EmployeeType Type { get; private set; }

        #endregion Properties

        #region Methods

        public override ValueTask<bool> IsDeletableAsync(DeleteType type, DbContext service)
        {
            //DOTO: Allow to delete if there Employee is new
            return new ValueTask<bool>(true);
        }

        public void PromoteTo(EmployeeType type, string userName)
        {
            Type = type;
            SetUpdatedBy(userName);
        }

        #endregion Methods
    }
}