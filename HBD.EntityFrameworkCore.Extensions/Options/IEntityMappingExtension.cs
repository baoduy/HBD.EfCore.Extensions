using System.Reflection;

namespace HBD.EntityFrameworkCore.Extensions.Options
{
    public interface IEntityMappingExtension
    {
        #region Public Methods

        /// <summary>
        /// The Assemblies will be scan
        /// </summary>
        /// <param name="entityAssemblies"></param>
        /// <returns></returns>
        RegistrationInfo FromAssemblies(params Assembly[] entityAssemblies);

        #endregion Public Methods
    }
}