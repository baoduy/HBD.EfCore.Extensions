using System;
using System.Reflection;
using HBD.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace HBD.EfCore.Hooks.Triggers
{
    public static class TriggerExtensions
    {
        /// <summary>
        /// Passing the Service Provider to Trigger Context
        /// </summary>
        /// <param name="service"></param>
        /// <param name="assemblies">The assemblies that contain your <see cref="ITriggerProfile"/> </param>
        /// <returns></returns>
        internal static IServiceCollection UseTrigger(this IServiceCollection service, params Assembly[] assemblies)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            var interfaceType = typeof(ITriggerProfile);
            var profiles = assemblies.Extract().Class().NotAbstract().IsInstanceOf(interfaceType);

            foreach (var p in profiles)
                service.Add(ServiceDescriptor.Singleton(interfaceType, p));

            return service;
        }
    }
}
