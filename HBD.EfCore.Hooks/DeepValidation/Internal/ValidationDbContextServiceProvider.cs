using System;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Hooks.DeepValidation.Internal
{
    /// <inheritdoc />
    /// <summary>
    /// This is the serviceProvider used in the validation of data in SaveChanges
    /// It allows the developer to access the current DbContext in the IValidateObject
    /// </summary>
    internal class ValidationDbContextServiceProvider : IServiceProvider
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// This creates the validation service provider
        /// </summary>
        /// <param name="dbContext">The current DbContext in which this validation is happening</param>
        public ValidationDbContextServiceProvider(DbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        /// <summary>
        /// This implements the GetService part of the service provider. It only understands the type DbContext
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType) => serviceType == typeof(DbContext) ? _dbContext : null;
    }
}
