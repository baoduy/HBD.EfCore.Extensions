using System;

namespace HBD.EfCore.Extensions.Attributes
{
    /// <summary>
    /// Allow to ignore the Entity class from auto mapper
    /// This should be use for delivered types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class IgnoreMapperAttribute : Attribute
    {
    }
}