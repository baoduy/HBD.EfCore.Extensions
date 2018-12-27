using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public interface ITypeExtractor : IEnumerable<Type>
    {
        #region Public Methods

        ITypeExtractor Abstract();

        ITypeExtractor Class();

        ITypeExtractor Enum();

        ITypeExtractor Generic();

        ITypeExtractor HasAttribute<TAttribute>() where TAttribute : Attribute;

        ITypeExtractor HasAttribute(Type attributeType);

        ITypeExtractor Interface();

        ITypeExtractor IsInstanceOf(Type type);

        ITypeExtractor IsInstanceOf<T>();

        ITypeExtractor Nested();

        ITypeExtractor NotAbstract();

        ITypeExtractor NotClass();

        ITypeExtractor NotEnum();

        ITypeExtractor NotGeneric();

        ITypeExtractor NotInstanceOf(Type type);

        ITypeExtractor NotInstanceOf<T>();

        ITypeExtractor NotInterface();

        ITypeExtractor NotNested();

        ITypeExtractor NotPublic();

        ITypeExtractor Public();

        ITypeExtractor Where(Expression<Func<Type, bool>> predicate);

        #endregion Public Methods
    }
}