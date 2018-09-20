using HBD.EntityFrameworkCore.Extensions.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HBD.EntityFrameworkCore.Extensions.Mappers
{
    public class EntityMapper<TEntity> : IEntityMapper<TEntity>
        where TEntity : class
    {
        public virtual void Map(EntityTypeBuilder<TEntity> builder)
        {
        }
    }
}
