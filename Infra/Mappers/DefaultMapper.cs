using HBD.EfCore.Extensions.Configurations;
using Domains.Abstracts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repos.Mappers
{
    internal class DefaultMapper<T> : EntityTypeConfiguration<T> where T : EntityBase
    {
        #region Methods

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
        }

        #endregion Methods
    }
}