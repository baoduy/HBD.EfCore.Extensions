using HBD.EfCore.Hooks.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace HBD.EfCore.Hooks.Internal
{
    internal class ServiceInjectionExtension : InternalDbContextOptionsExtension, IServiceInjectionExtension
    {
        #region Private Fields

        private readonly IList<Action<IServiceCollection>> _factories = new List<Action<IServiceCollection>>();

        #endregion Private Fields

        #region Public Methods

        public override bool ApplyServices(IServiceCollection services)
        {
            foreach (var factory in _factories)
                factory(services);

            _factories.Clear();
            return base.ApplyServices(services);
        }

        public void Includes(Action<IServiceCollection> factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _factories.Add(factory);
        }

        #endregion Public Methods
    }
}