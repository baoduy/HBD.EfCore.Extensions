using HBD.EfCore.Extensions.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataLayer.Specs
{
    public class UserIdGreaterThan10Spec : Spec<User>
    {
        #region Methods

        public override IQueryable<User> Includes(IQueryable<User> query) => query.Include(u => u.Addresses);

        public override Expression<Func<User, bool>> ToExpression() => u => u.Id > 10;

        #endregion Methods
    }
}