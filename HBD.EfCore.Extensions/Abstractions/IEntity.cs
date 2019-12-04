using HBD.EfCore.Extensions.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.EfCore.Extensions.Abstractions
{
    public interface IEntity<out TKey>
    {
        #region Properties

        [Key, Column(Order = 1)] [IgnoreFromUpdate] TKey Id { get; }

        #endregion Properties
    }

    public interface IEntity : IEntity<int>
    {
    }
}