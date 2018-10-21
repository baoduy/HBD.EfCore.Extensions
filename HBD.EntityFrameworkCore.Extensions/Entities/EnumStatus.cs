using System.ComponentModel.DataAnnotations.Schema;
using HBD.EntityFrameworkCore.Extensions.Attributes;

namespace DataLayer
{
    public enum EnumStatus
    {
        UnKnow,
        Active,
        InActive,
    }

    [Table(nameof(EnumStatus))]
    [StaticDataOf(typeof(EnumStatus))]
    internal class EnumStatusTable
    { }
}
