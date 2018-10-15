using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public interface IEntity<out TKey>
    {
        [Key]
        TKey Id { get; }

       
    }

    public interface IEntity : IEntity<long>
    {

    }


    
}
