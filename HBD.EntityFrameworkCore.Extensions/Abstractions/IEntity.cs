using System.ComponentModel.DataAnnotations;
using HBD.EntityFrameworkCore.Extensions.Attributes;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public interface IEntity<out TKey>
    {
        [Key]
        [IgnoreFromUpdate]
        TKey Id { get; }
       
    }

    public interface IEntity : IEntity<long>
    {

    }


    
}
