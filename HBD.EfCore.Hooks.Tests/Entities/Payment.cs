using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.EfCore.Hooks.Tests.Entities
{
    public class Payment
    {
        #region Public Constructors

        public Payment()
        {
            PaidOn = DateTime.Now;
            this.Id = Guid.NewGuid().ToString();
        }

        #endregion Public Constructors

        #region Public Properties

        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }

        public string Id { get; private set; }
        public DateTime PaidOn { get; private set; }

        #endregion Public Properties
    }
}