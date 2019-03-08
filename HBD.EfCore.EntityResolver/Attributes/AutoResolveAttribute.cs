using HBD.EfCore.Extensions.Specification;
using System;

namespace HBD.EfCore.EntityResolver.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Mark the Property that will be transformed to Entity
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AutoResolveAttribute : Attribute
    {
        #region Public Constructors
        /// <summary>
        /// Resolve with entity type if entity type is not provided it will be find from Linked interface of Property Type
        /// </summary>
        /// <param name="entityType"></param>
        public AutoResolveAttribute(Type entityType = null) => EntityType = entityType;

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// The Type of Entity will be resolved.
        /// </summary>
        public Type EntityType { get; internal set; }

        /// <summary>
        /// The type of <see cref="Spec{T}"/> which will be use to load Entity from Db instead of
        /// Find(Id). The <see cref="Spec{T}"/> must have a constructor with 1 parameter type is the
        /// same with the property type.
        /// </summary>
        public Type SpecType { get; set; }

        #endregion Public Properties
    }
}