using FluentAssertions;
using HBD.EfCore.Hooks.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.EfCore.Hooks.Tests
{
    [TestClass]
    public class DemoCalculationPropertiesTests : TestBase
    {
        #region Methods

        [TestMethod]
        public async Task TotalAmount_GotCalCalculated_PaymentChanges()
        {
            var db = GetService<TestHookDbContext>();
            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
                Payments = { new Payment { Amount = 100 } }
            };

            db.Add(user);
            await db.SaveChangesAsync().ConfigureAwait(false);

            user.TotalPaymentCalculated.Should().BeTrue();
            user.TotalPayment.Should().Be(user.Payments.Sum(i => i.Amount));
            user.Validated.Should().BeTrue();
        }

        [TestMethod]
        public async Task TotalAmount_NotCalCalculated_When_PaymentNotChanges()
        {
            await TotalAmount_GotCalCalculated_PaymentChanges().ConfigureAwait(false);
            var db = GetService<TestHookDbContext>();

            var user = db.Set<User>().First();

            //Reset Checking state of User
            user.Reset();

            //Ensure TotalPaymentCalculated = false
            user.TotalPaymentCalculated.Should().BeFalse();
            user.TotalPayment.Should().Be(100);

            //Save changes
            await db.SaveChangesAsync().ConfigureAwait(false);

            //the calculation of TotalPayment should not be performed.
            user.TotalPaymentCalculated.Should().BeFalse();
            user.TotalPayment.Should().Be(100);
        }

        [TestMethod]
        public async Task TotalAmount_ReCalCalculated_When_PaymentChanges()
        {
            await TotalAmount_GotCalCalculated_PaymentChanges().ConfigureAwait(false);
            var db = GetService<TestHookDbContext>();

            var user = db.Set<User>().Include(u => u.Payments).First();

            user.Reset();

            //Ensure TotalPaymentCalculated = false
            user.TotalPaymentCalculated.Should().BeFalse();
            user.TotalPayment.Should().Be(100);

            //Update user Payment
            user.AddPayment(new Payment { Amount = 100 });

            //Save changes
            await db.SaveChangesAsync().ConfigureAwait(false);

            //the calculation of TotalPayment should not be performed.
            user.TotalPayment.Should().Be(user.Payments.Sum(i => i.Amount));
            user.TotalPaymentCalculated.Should().BeTrue();
        }

        #endregion Methods
    }
}