using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataLayer
{
    public class Account: AuditEntity
    {
        //public long UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
