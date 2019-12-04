using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HBD.EfCore.Hooks.DeepValidation.Internal;
using HBD.EfCore.Hooks.Triggers;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks.DeepValidation
{
    public sealed class DeepValidationHook : Hook
    {
        #region Fields

        private readonly SetupOptions _options;

        #endregion Fields

        #region Constructors

        public DeepValidationHook(SetupOptions options) => _options = options;

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Disable DeepValidationHook
        /// </summary>
        public static bool Disabled { get; set; }

        #endregion Properties

        #region Methods

        public override Task OnSaving(IReadOnlyCollection<TriggerEntityState> entities, DbContext dbContext, CancellationToken cancellationToken = default)
        {
            if (dbContext is null)
            {
                throw new System.ArgumentNullException(nameof(dbContext));
            }

            if (Disabled) return Task.CompletedTask;

            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            var status = new List<ValidationResult>();
            var validatingEntities = entities.GetEntitiesForValidation();

            try
            {
                var valProvider = new ValidationDbContextServiceProvider(dbContext);
                var errors = new List<ValidationResult>();

                foreach (var entity in validatingEntities)
                {
                    //Stop validation if any error found
                    if (status.Count > 0) break;

                    var valContext = new ValidationContext(entity, valProvider, null);
                    errors.Clear();

                    if (!Validator.TryValidateObject(entity, valContext, errors, _options.ValidateAllProperties))
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

        #endregion Methods
    }
}