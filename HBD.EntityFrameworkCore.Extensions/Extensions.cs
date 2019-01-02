using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HBD.EntityFrameworkCore.Extensions.Abstractions;
using HBD.EntityFrameworkCore.Extensions.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HBD.EntityFrameworkCore.Extensions
{
    public static class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Update the values from obj for Owned type ONLY. Those properties marked as
        /// [ReadOnly(true)] or
        /// TODO: Should improve the performance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="obj"></param>
        /// <param name="ignoreNull"></param>
        /// <param name="bindingFlags"></param>
        public static T UpdateFrom<T>(this T @this, T obj, bool ignoreNull = false,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public) where T : class
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (typeof(T).GetCustomAttribute<OwnedAttribute>() == null)
                throw new ArgumentException("Only Owned Type is accepted");

            foreach (var property in obj.GetType().GetProperties(bindingFlags))
            {
                var readOnly = property.GetCustomAttribute<ReadOnlyAttribute>();
                var ignored = property.GetCustomAttribute<IgnoreFromUpdateAttribute>();

                if (ignored != null
                    || readOnly?.IsReadOnly == true
                    || !property.CanRead
                    || !property.CanWrite) continue;

                var val = property.GetValue(obj);

                if (val == null)
                {
                    if (ignoreNull)
                        continue;

                    property.SetValue(@this, null);
                    continue;
                }

                property.SetValue(@this, val);
            }

            return @this;
        }

        #endregion Public Methods

        #region Internal Methods

        internal static Type GetEntityType(Type entityMappingType)
                    => entityMappingType.GetInterfaces().First(a => a.IsGenericType).GetGenericArguments().First();

        #endregion Internal Methods

        /// <summary>
        /// Get Property Name of expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetPropName<T, TProp>(this Expression<Func<T, TProp>> action) where T : class
        {
            var expression = GetMemberInfo(action);
            return expression.Member.Name;
        }

        /// <summary>
        /// Get Expression member
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private static MemberExpression GetMemberInfo(Expression method)
        {
            if (!(method is LambdaExpression lambda))
                throw new ArgumentNullException(nameof(method));

            MemberExpression memberExpr = null;

            switch (lambda.Body.NodeType)
            {
                case ExpressionType.Convert:
                    memberExpr =
                        ((UnaryExpression)lambda.Body).Operand as MemberExpression;
                    break;
                case ExpressionType.MemberAccess:
                    memberExpr = lambda.Body as MemberExpression;
                    break;
            }

            if (memberExpr == null)
                throw new ArgumentException(nameof(method));

            return memberExpr;
        }

        /// <summary>
        /// Check whether the property value had been Add or Updated
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="props"></param>
        /// <returns></returns>
        public static bool HasChangeOn<T>(this EntityEntry<T> @this, Expression<Func<T, object>> props) where T : class
        {
            var name = props.GetPropName();
            var prop = @this.Properties.FirstOrDefault(i => i.Metadata.Name == name);

            if (prop != null) return prop.IsModified || @this.State == EntityState.Added;

            var navi = @this.Navigations.FirstOrDefault(n => n.Metadata.Name == name);

            if (navi != null)
            {
                if (navi.EntityEntry.State == EntityState.Added)
                    return true;

                if (!navi.IsLoaded) return false;
                return navi.IsModified;
            }

            return @this.State == EntityState.Added;
        }
    }
}