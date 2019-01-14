using System;
using System.Linq;
using System.Linq.Expressions;
using DataLayer;
using HBD.EntityFrameworkCore.Extensions.Specification;
using Microsoft.EntityFrameworkCore;

namespace HBD.EntityFrameworkCore.Extensions.Tests.TestClasses
{
    internal class UserAccountStartWithDSpec : Spec<User>
    {
        public override IQueryable<User> Includes(IQueryable<User> query) => 
            query.Include(i => i.Account);

        public override Expression<Func<User, bool>> ToExpression() => u => u.Account.UserName.StartsWith("D");
    }
}
