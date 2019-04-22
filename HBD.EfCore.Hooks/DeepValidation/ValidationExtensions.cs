using HBD.EfCore.Hooks.Triggers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HBD.EfCore.Hooks.DeepValidation
{
    public static class ValidationExtensions
    {
        public static IEnumerable<object> GetEntitiesForValidation(this IReadOnlyCollection<TriggerEntityState> entities)
            => (from e in entities
               where e.Entity is IValidatableObject && (e.State == EntityState.Added || e.State == EntityState.Modified)
               select e.Entity).Distinct();

    }
}
