using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HBD.EfCore.Extensions.Attributes;

namespace HBD.EfCore.Extensions.Abstractions
{
    public interface IEntity<out TKey>
    {
        #region Public Properties

        [Key, Column(Order = 1)] [IgnoreFromUpdate] TKey Id { get; }

        #endregion Public Properties
    }

    public interface IEntity : IEntity<int>
    {
    }
}