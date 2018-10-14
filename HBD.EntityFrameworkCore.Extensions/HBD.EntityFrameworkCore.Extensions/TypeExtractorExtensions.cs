using System;
using System.Collections.Generic;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using HBD.EntityFrameworkCore.Extensions.Internal;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class TypeExtractorExtensions
    {
        public static ITypeExtractor Extract(this Assembly assembly)
            => new[] {assembly}.Extract();

        public static ITypeExtractor Extract(this Assembly[] assemblies)
            => new TypeExtractor(assemblies);

        /// <summary>
        /// Get Public classes which name contains the nameContains
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="nameContains"></param>
        /// <returns></returns>
        public static IEnumerable<Type> ScanGenericClassesWithFilter(this Assembly[] assemblies, string nameContains)
            => assemblies.Extract().Generic().Class().NotAbstract()
                .Where(t => t.Name.Contains(nameContains));

        public static IEnumerable<Type> ScanGenericClassesWithFilter(this Assembly assembly, string nameContains)
            => new[] { assembly }.ScanGenericClassesWithFilter(nameContains);

        /// <summary>
        /// Get Public classes which name contains the nameContains
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="nameContains"></param>
        /// <returns></returns>
        public static IEnumerable<Type> ScanPublicClassesWithFilter(this Assembly[] assemblies, string nameContains)
            => assemblies.Extract().Public().Class().NotAbstract().NotGeneric()
            .Where(t => t.Name.Contains(nameContains));

        public static IEnumerable<Type> ScanPublicClassesWithFilter(this Assembly assembly, string nameContains)
            => new[] { assembly }.ScanPublicClassesWithFilter(nameContains);

        /// <summary>
        /// Get Public and Private classes which name contains the nameContains
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="nameContains"></param>
        /// <returns></returns>
        public static IEnumerable<Type> ScanClassesWithFilter(this Assembly[] assemblies, string nameContains)
            => assemblies.Extract().Class().NotAbstract().NotGeneric()
                .Where(t => t.Name.Contains(nameContains));

        public static IEnumerable<Type> ScanClassesWithFilter(this Assembly assembly, string nameContains)
            => new[] { assembly }.ScanClassesWithFilter(nameContains);

        /// <summary>
        /// Get Public classes which implement of T
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IEnumerable<Type> ScanPublicClassesImplementOf<T>(this Assembly[] assemblies)
            => assemblies.ScanPublicClassesImplementOf(typeof(T));

        public static IEnumerable<Type> ScanPublicClassesImplementOf<T>(this Assembly assembly)
            => new[] { assembly }.ScanPublicClassesImplementOf<T>();

        /// <summary>
        /// Get Public classes which implement of type
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> ScanPublicClassesImplementOf(this Assembly[] assemblies, Type type)
            => assemblies.Extract().Public().Class().NotAbstract().NotGeneric()
                .IsInstanceOf(type);

        public static IEnumerable<Type> ScanPublicClassesImplementOf(this Assembly assembly, Type type)
            => new[] { assembly }.ScanPublicClassesImplementOf(type);

        /// <summary>
        /// Get Public and Private classes which implement of T
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IEnumerable<Type> ScanClassesImplementOf<T>(this Assembly[] assemblies)
            => assemblies.ScanClassesImplementOf(typeof(T));

        public static IEnumerable<Type> ScanClassesImplementOf<T>(this Assembly assembly)
            => new []{assembly}.ScanClassesImplementOf<T>();

        /// <summary>
        /// Get Public and Private classes which implement of T
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> ScanClassesImplementOf(this Assembly[] assemblies, Type type)
            => new TypeExtractor(assemblies).Class().NotAbstract().NotGeneric()
                .IsInstanceOf(type);

        public static IEnumerable<Type> ScanClassesImplementOf(this Assembly assembly, Type type)
            => new[] {assembly}.ScanClassesImplementOf(type);
    }
}
