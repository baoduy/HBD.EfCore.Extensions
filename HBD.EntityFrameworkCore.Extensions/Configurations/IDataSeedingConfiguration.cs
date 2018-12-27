namespace HBD.EntityFrameworkCore.Extensions.Configurations
{
    public interface IDataSeedingConfiguration<TEntity> where TEntity : class
    {
        #region Public Properties

        //void Apply(EntityTypeBuilder<TEntity> builder);
        TEntity[] Data { get; }

        #endregion Public Properties
    }
}