using System.Reflection;

namespace HBD.EfCore.Extensions.Options
{
    public interface IEntityMappingExtension
    {
        #region Public Methods

        /// <summary>
        /// The Assemblies will be scan
        /// </summary>
        /// <param name="entityAssemblies"></param>
        /// <returns></returns>
        RegistrationInfo ScanFrom(params Assembly[] entityAssemblies);

        #endregion Public Methods
    }
}