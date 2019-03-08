using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HBD.EfCore.Hooks.DeepValidation.Internal;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks.DeepValidation
{
    internal class DeepValidationHook : Hook
    {
        public override Task OnSaving(IReadOnlyCollection<EntityInfo> entities, DbContext dbContext, CancellationToken cancellationToken = default)
        {
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            var status = new List<ValidationResult>();
            var validatingEntities = from e in entities
                                     where e.Entity is IValidatableObject && (e.State == EntityState.Added || e.State == EntityState.Modified)
                                     select e.Entity;
            try
            {
                var valProvider = new ValidationDbContextServiceProvider(dbContext);
                var errors = new List<ValidationResult>();

                foreach (var entity in validatingEntities)
                {
                    var valContext = new ValidationContext(entity, valProvider, null);
                    errors.Clear();

                    if (!Validator.TryValidateObject(entity, valContext, errors, true))
                        status.AddRange(errors);
                }
            }
            finally
            {
                dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }

            if (status.Any())
                throw new ValidationException(status);

            return base.OnSaving(entities, dbContext, cancellationToken);
        }
    }
}
