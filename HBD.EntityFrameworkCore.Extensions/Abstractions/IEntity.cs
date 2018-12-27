using System.ComponentModel.DataAnnotations;
using HBD.EntityFrameworkCore.Extensions.Attributes;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
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