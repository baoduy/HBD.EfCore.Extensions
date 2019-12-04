using HBD.EfCore.Extensions.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataLayer.Specs
{
    public class UserIncludeAccountSpec : Spec<User>
    {
        #region Methods

        public override IQueryable<User> Includes(IQueryable<User> query)
            => query.Include(u => u.Account);

        public override Expression<Func<User, bool>> ToExpression()
            => All.ToExpression();

        #endregion Methods
    }
}