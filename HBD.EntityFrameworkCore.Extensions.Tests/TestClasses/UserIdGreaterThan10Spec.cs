using System;
using System.Linq;
using System.Linq.Expressions;
using DataLayer;
using HBD.EntityFrameworkCore.Extensions.Specification;
using Microsoft.EntityFrameworkCore;

namespace HBD.EntityFrameworkCore.Extensions.Tests.TestClasses
{
    internal class UserIdGreaterThan10Spec : Spec<User>
    {
        #region Public Properties

        public override IQueryable<User> Includes(IQueryable<User> query) => query.Include(u => u.Addresses);

        #endregion Public Properties

        #region Public Methods

        public override Expression<Func<User, bool>> ToExpression() => u => u.Id > 10;

        #endregion Public Methods
    }
}