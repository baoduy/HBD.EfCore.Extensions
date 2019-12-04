using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Domains.Accounts;
using HBD.EfCore.Extensions;
using Domains.Abstracts;
using Domains.Owned;
using Microsoft.EntityFrameworkCore;

namespace Domains.Profiles
{
    [Table("Profiles", Schema = DomainSchemas.Profile)]
    public class Profile : EntityBase
    {
        #region Fields

        private HashSet<Account> _accounts = new HashSet<Account>();

        #endregion Fields

        #region Constructors

        public Profile(PersionName name, string email, string mobile, string avatar, DateTime birthday, string userName)
        {
            UpdateName(name ?? new PersionName(), userName);

            Email = email;
            Mobile = mobile;

            Update(avatar, birthday, userName);
        }

        private Profile()
        {
        }

        #endregion Constructors

        #region Properties

        public IReadOnlyCollection<Account> Accounts => _accounts;

        [MaxLength(50)]
        public string Avatar { get; private set; }

        public DateTime BirthDay { get; private set; }

        [StringLength(150)]
        [EmailAddress]
        [Required]
        public string Email { get; private set; }

        [Phone]
        [MaxLength(50)]
        public string Mobile { get; private set; }

        public PersionName Name { get; private set; }

        #endregion Properties

        #region Methods

        public void AddAccount(Account account, string userName)
        {
            _accounts.Add(account);
            SetUpdatedBy(userName);
        }

        public override ValueTask<bool> IsDeletableAsync(DeleteType type, DbContext service) => new ValueTask<bool>(false);

        public void RemoveAccount(Account account, string userName)
        {
            _accounts.RemoveWhere(a => a == account || a.Id == account.Id);
            SetUpdatedBy(userName);
        }

        public void Update(string avatar, DateTime birthday, string userName)
        {
            Avatar = avatar;
            BirthDay = birthday;
            SetUpdatedBy(userName);
        }

        public void UpdateName(PersionName name, string userName)
        {
            Name.UpdateFrom(name);
            SetUpdatedBy(userName);
        }

        #endregion Methods
    }
}