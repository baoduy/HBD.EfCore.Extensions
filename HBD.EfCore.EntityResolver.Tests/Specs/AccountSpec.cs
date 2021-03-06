﻿using System;
using System.Linq.Expressions;
using HBD.EfCore.EntityResolver.Tests.Models;
using HBD.EfCore.Extensions.Specification;
using HBD.Framework.Extensions;
using Account = HBD.EfCore.EntityResolver.Tests.Entities.Account;

namespace HBD.EfCore.EntityResolver.Tests.Specs
{
    internal sealed class AccountSpec : Spec<Account>
    {
        #region Fields

        private readonly int _accountId;
        private readonly string _userName;

        #endregion Fields

        #region Constructors

        public AccountSpec(int accountId) => _accountId = accountId;

        public AccountSpec(AccountBasicViewModel accountModel)
        {
            _accountId = accountModel.Id;
            _userName = accountModel.UserName;
        }

        #endregion Constructors

        #region Methods

        public override Expression<Func<Account, bool>> ToExpression()
        {
            if (_userName.IsNullOrEmpty())
                return a => a.Id == _accountId;

            return a => a.Id == _accountId && a.UserName == _userName;
        }

        #endregion Methods
    }
}