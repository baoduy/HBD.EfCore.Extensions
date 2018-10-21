using System;

namespace HBD.EntityFrameworkCore.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class StaticDataOfAttribute : Attribute
    {
        internal Type EnumType { get; }

        public StaticDataOfAttribute(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException(nameof(enumType));

            if (!enumType.IsEnum)
                throw new ArgumentException($"The {enumType.Name} is not a type of Enum");

            EnumType = enumType;
        }
    }
}
