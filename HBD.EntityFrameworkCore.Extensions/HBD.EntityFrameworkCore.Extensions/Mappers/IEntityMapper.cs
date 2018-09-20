using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HBD.EntityFrameworkCore.Extensions.Mappers
{
    public interface IEntityMapper<TEntity> where TEntity : class
    {
        void Map(EntityTypeBuilder<TEntity> builder);
    }
}
