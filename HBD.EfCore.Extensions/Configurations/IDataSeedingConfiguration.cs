using System.Collections.Generic;

namespace HBD.EfCore.Extensions.Configurations
{
    public interface IDataSeedingConfiguration<TEntity> where TEntity : class
    {
        #region Properties

        //void Apply(EntityTypeBuilder<TEntity> builder);
        ICollection<TEntity> Data { get; }

        #endregion Properties
    }
}