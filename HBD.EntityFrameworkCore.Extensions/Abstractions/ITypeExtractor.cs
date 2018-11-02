using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public interface ITypeExtractor : IEnumerable<Type>
    {
        ITypeExtractor Abstract();
        ITypeExtractor NotAbstract();

        ITypeExtractor Class();
        ITypeExtractor NotClass();

        ITypeExtractor Generic();
        ITypeExtractor NotGeneric();

        ITypeExtractor Interface();
        ITypeExtractor NotInterface();

        ITypeExtractor IsInstanceOf(Type type);
        ITypeExtractor NotInstanceOf(Type type);

        ITypeExtractor IsInstanceOf<T>();
        ITypeExtractor NotInstanceOf<T>();

        ITypeExtractor Nested();
        ITypeExtractor NotNested();

        ITypeExtractor NotPublic();
        ITypeExtractor Public();

        ITypeExtractor Enum();
        ITypeExtractor NotEnum();

        ITypeExtractor HasAttribute<TAttribute>() where TAttribute : Attribute;
        ITypeExtractor HasAttribute(Type attributeType);

        ITypeExtractor Where(Expression<Func<Type, bool>> predicate);
    }
}