using HBD.EfCore.Extensions.Abstractions;

namespace HBD.EfCore.Extensions.Internal
{
    internal sealed class EnumTables<T> : IEntity<int>
    {
        #region Properties

        public int Id { get; private set; }

        #endregion Properties
    }
}