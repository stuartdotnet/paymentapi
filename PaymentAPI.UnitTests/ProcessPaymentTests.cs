
using Moq;
using NUnit.Framework;
using PaymentAPI.Business;
using PaymentAPI.Data;
using PaymentAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.UnitTests
{
    public class ProcessPaymentTests
    {
        private Mock<IPaymentData> _paymentData;
        private InMemoryTestAccountData _accountData;

        [SetUp]
        public void Setup()
        {
            _paymentData = new Mock<IPaymentData>();
            _accountData = new InMemoryTestAccountData
            {
                Accounts = new List<Account>
                {
                    new Account { AccountId = 1, Balance = 200 },
                    new Account { AccountId = 2, Balance = 300 },
                    new Account { AccountId = 3, Balance = 400 }
                }
             };

            _paymentData.Setup(p => p.Get(1)).ReturnsAsync(new PaymentRequest { PaymentRequestId = 1, AccountId = 1, Account = _accountData.Accounts.Single(a => a.AccountId == 1), Date = new DateTime(2020, 1, 1), Amount = 150, Status = PaymentStatus.Pending });
            _paymentData.Setup(p => p.Get(2)).ReturnsAsync(new PaymentRequest { PaymentRequestId = 2, AccountId = 1, Account = _accountData.Accounts.Single(a => a.AccountId == 1), Date = new DateTime(2020, 1, 2), Amount = 200, Status = PaymentStatus.Processed });
            _paymentData.Setup(p => p.Get(3)).ReturnsAsync(new PaymentRequest { PaymentRequestId = 3, AccountId = 3, Account = _accountData.Accounts.Single(a => a.AccountId == 3), Date = new DateTime(2020, 1, 3), Amount = 500, Status = PaymentStatus.Pending });

            _paymentData.Setup(p => p.Commit()).ReturnsAsync(1); // mock successful db save of 1 record
        }

        [Test]
        public async Task ProcessPayment_WhenPending_ProcessesPayment()
        {
            var sut = new PaymentService(_paymentData.Object, _accountData);

            var result = await sut.ProcessPaymentRequest(1);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(PaymentStatus.Processed, result.Entity.Status);
        }

        [Test]
        public async Task ProcessPayment_WhenPending_UpdatesBalance()
        {
            var sut = new PaymentService(_paymentData.Object, _accountData);

            var result = await sut.ProcessPaymentRequest(1); // balance of 200, payment of 150

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(PaymentStatus.Processed, result.Entity.Status);
            Assert.AreEqual(50, result.Entity.Account.Balance);
        }

        [Test]
        public async Task ProcessPayment_WhenInsufficientBalance_FailsUpdate()
        {
            var sut = new PaymentService(_paymentData.Object, _accountData);

            var result = await sut.ProcessPaymentRequest(3); // balance of 400, payment of 500

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(PaymentStatus.Closed, result.Entity.Status);
            Assert.AreEqual(400, result.Entity.Account.Balance); // unchanged
        }

        [Test]
        public async Task ProcessPayment_WhenProcessed_Fails()
        {
            var sut = new PaymentService(_paymentData.Object, _accountData);

            var result = await sut.ProcessPaymentRequest(2);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(PaymentStatus.Processed, result.Entity.Status);
        }

        [Test]
        public async Task ProcessPayment_WhenClosed_Fails()
        {
            var sut = new PaymentService(_paymentData.Object, _accountData);

            var result = await sut.ProcessPaymentRequest(3);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(PaymentStatus.Closed, result.Entity.Status);
        }
    }
}