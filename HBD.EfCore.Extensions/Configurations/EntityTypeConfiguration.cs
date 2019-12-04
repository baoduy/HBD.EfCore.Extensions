using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HBD.EfCore.Extensions.Configurations
{
    public class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        #region Methods

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
        }

        #endregion Methods
    }
}