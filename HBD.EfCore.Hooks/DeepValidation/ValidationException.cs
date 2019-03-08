using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HBD.EfCore.Hooks.DeepValidation
{
    public class ValidationException : Exception
    {
        public IList<ValidationResult> Results { get; }

        public ValidationException(IList<ValidationResult> results) => Results = results;
    }
}
