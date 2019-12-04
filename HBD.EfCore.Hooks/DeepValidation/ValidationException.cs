using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HBD.EfCore.Hooks.DeepValidation
{
    public class ValidationException : Exception
    {
        #region Constructors

        public ValidationException(IList<ValidationResult> results) => Results = results;

        public ValidationException()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors

        #region Properties

        public IList<ValidationResult> Results { get; }

        #endregion Properties
    }
}