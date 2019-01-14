using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EfCore.Extensions.Tests
{
    [TestClass]
    public class DemoCalculationProperties
    {
        [TestMethod]
        public async Task TotalAmount_GotCalCalculated_PaymentChanges()
        {
            var user = new User
            {
                FirstName = "Duy",
                LastName = "Hoang",
                Addresses = { new Address { Street = "111" } },
                Payments = { new Payment { Amount = 100 } }
            };

            UnitTestSetup.Db.Add(user);
            await UnitTestSetup.Db.SaveChangesAsync();

            user.TotalPaymentCalculated.Should().BeTrue();
            user.TotalPayment.Should().Be(user.Payments.Sum(i => i.Amount));
        }

        [TestMethod]
        public async Task TotalAmount_NotCalCalculated_When_PaymentNotChanges()
        {
            await TotalAmount_GotCalCalculated_PaymentChanges();

            var user = UnitTestSetup.Db.Set<User>().Last();
            //Reset Checking state of User
            user.Reset();
            //Ensure TotalPaymentCalculated = false
            user.TotalPaymentCalculated.Should().BeFalse();
            user.TotalPayment.Should().Be(100);

            //Update user Address
            user.Addresses.Add(new Address { Street = "123" });
            //Save changes
            await UnitTestSetup.Db.SaveChangesAsync();
            //the calculation of TotalPayment should not be performed.
            user.TotalPaymentCalculated.Should().BeFalse();
            user.TotalPayment.Should().Be(100);
        }

        [TestMethod]
        public async Task TotalAmount_ReCalCalculated_When_PaymentChanges()
        {
            await TotalAmount_GotCalCalculated_PaymentChanges();

            var user = UnitTestSetup.Db.Set<User>().Include(u => u.Payments).Last();
            //Reset Checking state of User
            user.Reset();

            //Ensure TotalPaymentCalculated = false
            user.TotalPaymentCalculated.Should().BeFalse();
            user.TotalPayment.Should().Be(100);

            //Update user Payment
            user.Payments.Add(new Payment { Amount = 100 });
            //Save changes
            await UnitTestSetup.Db.SaveChangesAsync();
            //the calculation of TotalPayment should not be performed.
            user.TotalPaymentCalculated.Should().BeTrue();
            user.TotalPayment.Should().Be(200);
        }
    }
}
