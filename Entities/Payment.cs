using System;
using System.ComponentModel.DataAnnotations.Schema;
using HBD.EntityFrameworkCore.Extensions.Abstractions;

namespace DataLayer
{
    public class Payment : Entity<string>
    {
        public Payment() : base(Guid.NewGuid().ToString())
        {
            PaidOn = DateTime.Now;
        }

        public DateTime PaidOn { get; private set; }

        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }


    }
}
