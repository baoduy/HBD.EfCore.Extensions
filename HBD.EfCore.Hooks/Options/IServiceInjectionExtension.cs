using Microsoft.Extensions.DependencyInjection;
using System;

namespace HBD.EfCore.Hooks.Options
{
    internal interface IServiceInjectionExtension
    {
        #region Public Methods

        void Includes(Action<IServiceCollection> factory);

        #endregion Public Methods
    }
}