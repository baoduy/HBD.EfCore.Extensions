using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HBD.EfCore.Extensions.Internal
{
    internal class ExtraModelCustomizer : IModelCustomizer
    {
        private readonly IModelCustomizer _original;

        public ExtraModelCustomizer(IModelCustomizer original) => _original = original;

        public void Customize(ModelBuilder modelBuilder, DbContext context)
        {
            context.ConfigModelCreating(modelBuilder);
            _original?.Customize(modelBuilder, context);
        }
    }
}
