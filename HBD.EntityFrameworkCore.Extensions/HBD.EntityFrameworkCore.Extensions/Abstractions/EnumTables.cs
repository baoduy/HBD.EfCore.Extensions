namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public sealed class EnumTables<T> : IEntity<int>
    {
        public int Id { get; private set; }
    }
}
