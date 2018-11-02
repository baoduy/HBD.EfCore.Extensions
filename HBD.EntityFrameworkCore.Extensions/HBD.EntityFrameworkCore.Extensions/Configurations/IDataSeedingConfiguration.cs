namespace HBD.EntityFrameworkCore.Extensions.Configurations
{
    public interface IDataSeedingConfiguration<TEntity> where TEntity:class
    {
        //void Apply(EntityTypeBuilder<TEntity> builder);
        TEntity[] Data { get; }
    }
}
