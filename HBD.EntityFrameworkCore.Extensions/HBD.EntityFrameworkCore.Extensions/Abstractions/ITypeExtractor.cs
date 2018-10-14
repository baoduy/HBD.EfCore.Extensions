using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public interface ITypeExtractor: IEnumerable<Type>
    {
        ITypeExtractor Abstract();
        ITypeExtractor Class();
        ITypeExtractor Generic();
        ITypeExtractor Interface();
        ITypeExtractor IsInstanceOf(Type type);
        ITypeExtractor IsInstanceOf<T>();
        ITypeExtractor Nested();
        ITypeExtractor NotAbstract();
        ITypeExtractor NotClass();
        ITypeExtractor NotGeneric();
        ITypeExtractor NotInstanceOf(Type type);
        ITypeExtractor NotInstanceOf<T>();
        ITypeExtractor NotInterface();
        ITypeExtractor NotNested();
        ITypeExtractor NotPublic();
        ITypeExtractor Public();
        ITypeExtractor Where(Expression<Func<Type, bool>> predicate);
    }
}