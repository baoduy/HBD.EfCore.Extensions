using System.Runtime.CompilerServices;
using HBD.EfCore.Extensions.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: InternalsVisibleTo("HBD.EfCore.Extensions.Tests")]

namespace DataLayer.Mappers
{
    internal class BaseEntityMapper<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        #region Public Properties

        public static bool Called { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            Called = true;
            base.Configure(builder);
            builder.HasIndex(c => c.Id).IsUnique();
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
        }

        #endregion Public Methods
    }
}