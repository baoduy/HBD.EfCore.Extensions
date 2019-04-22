using System.ComponentModel.DataAnnotations;
using HBD.EfCore.Extensions.Attributes;

namespace DataLayer
{
    [StaticData]
    public enum EnumStatus
    {
        UnKnow,
        Active,
        InActive
    }

    [StaticData(Table = "EnumStatusOther")]
    public enum EnumStatus1
    {
        [Display(Name = "AA", Description = "BB")]
        UnKnow,
        Active,
        InActive
    }
}