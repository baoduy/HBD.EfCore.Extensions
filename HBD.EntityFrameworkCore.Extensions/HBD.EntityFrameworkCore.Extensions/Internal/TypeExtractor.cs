using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HBD.EntityFrameworkCore.Extensions.Internal
{
    internal class TypeExtractor : IEnumerable<Type>
    {
        private IQueryable<Type> _query;

        public TypeExtractor(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length <= 0)
                throw new ArgumentNullException(nameof(assemblies));

            _query = assemblies.SelectMany(a => a.GetTypes()).AsQueryable();
        }

        public TypeExtractor Class() => Where(t => t.IsClass);
        public TypeExtractor NotClass() => Where(t => !t.IsClass);

        public TypeExtractor Abstract() => Where(t => t.IsAbstract);
        public TypeExtractor NotAbstract() => Where(t => !t.IsAbstract);

        public TypeExtractor Generic() => Where(t => t.IsGenericType);
        public TypeExtractor NotGeneric() => Where(t => !t.IsGenericType);

        public TypeExtractor Nested() => Where(t => t.IsNested);
        public TypeExtractor NotNested() => Where(t => !t.IsNested);

        public TypeExtractor Interface() => Where(t => t.IsInterface);
        public TypeExtractor NotInterface() => Where(t => !t.IsInterface);

        public TypeExtractor Public() => Where(t => t.IsPublic);
        public TypeExtractor NotPublic() => Where(t => !t.IsPublic);

        public TypeExtractor IsInstanceOf(Type type)
            => type.IsGenericType && type.IsInterface
                ? Where(t => t.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == type))
                : Where(t => type.IsAssignableFrom(t));

        public TypeExtractor IsInstanceOf<T>() => IsInstanceOf(typeof(T));

        public TypeExtractor NotInstanceOf(Type type)
            => type.IsGenericType && type.IsInterface
                ? Where(t => !t.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == type))
                : Where(t => !type.IsAssignableFrom(t));

        public TypeExtractor NotInstanceOf<T>() => NotInstanceOf(typeof(T));

        public TypeExtractor Where(Expression<Func<Type, bool>> predicate)
        {
            if (predicate != null)
                _query = _query.Where(predicate);
            return this;
        }

        public IEnumerator<Type> GetEnumerator() => _query.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
