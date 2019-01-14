using System.ComponentModel.DataAnnotations;
using HBD.EfCore.Extensions.Attributes;

namespace HBD.EfCore.Extensions.Abstractions
{
    public interface IEntity<out TKey>
    {
        #region Public Properties

        [Key] [IgnoreFromUpdate] TKey Id { get; }

        #endregion Public Properties
    }

    public interface IEntity : IEntity<int>
    {
    }
}