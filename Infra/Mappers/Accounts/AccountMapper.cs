using Domains;
using Domains.Accounts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repos.Mappers.Accounts
{
    internal enum AccountTypes
    {
        Personal = 1,
        Business = 2,
    }

    internal sealed class AccountMapper : DefaultMapper<Account>
    {
        #region Methods

        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);

            builder.Property<int>(DomainConsts.FieldAccountType).IsRequired();

            builder.HasDiscriminator<AccountTypes>(DomainConsts.FieldAccountType)
                .HasValue<BusinessAccount>(AccountTypes.Business)
                .HasValue<PersonalAccount>(AccountTypes.Personal);
        }

        #endregion Methods
    }

    internal sealed class BusinessAccountMapper : DefaultMapper<BusinessAccount>
    {
        #region Methods

        public override void Configure(EntityTypeBuilder<BusinessAccount> builder)
            => builder.HasBaseType<Account>();

        #endregion Methods
    }

    internal sealed class PersonalAccountMapper : DefaultMapper<PersonalAccount>
    {
        #region Methods

        public override void Configure(EntityTypeBuilder<PersonalAccount> builder)
            => builder.HasBaseType<Account>();

        #endregion Methods
    }
}