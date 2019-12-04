using HBD.EfCore.Extensions.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataLayer.Specs
{
    public class UserAccountStartWithDSpec : Spec<User>
    {
        #region Methods

        public override IQueryable<User> Includes(IQueryable<User> query) =>
            query.Include(i => i.Account);

        public override Expression<Func<User, bool>> ToExpression() => u => u.Account.UserName.StartsWith("D", StringComparison.OrdinalIgnoreCase);

        #endregion Methods
    }
}