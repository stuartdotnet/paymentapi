
using Moq;
using NUnit.Framework;
using PaymentAPI.Business;
using PaymentAPI.Data;
using PaymentAPI.Model;
using System;
using System.Threading.Tasks;

namespace PaymentAPI.UnitTests
{
    public class CancelPaymentTests
    {
        private Mock<IPaymentData> _paymentData;
        private Mock<IAccountData> _accountData;

        [SetUp]
        public void Setup()
        {
            _paymentData = new Mock<IPaymentData>();
            _accountData = new Mock<IAccountData>();

            _paymentData.Setup(p => p.Get(1)).ReturnsAsync(new PaymentRequest { PaymentRequestId = 1, AccountId = 1, Date = new DateTime(2020, 1, 1), Amount = 100, Status = PaymentStatus.Pending });
            _paymentData.Setup(p => p.Get(2)).ReturnsAsync(new PaymentRequest { PaymentRequestId = 2, AccountId = 1, Date = new DateTime(2020, 1, 2), Amount = 200, Status = PaymentStatus.Processed });
            _paymentData.Setup(p => p.Get(3)).ReturnsAsync(new PaymentRequest { PaymentRequestId = 3, AccountId = 1, Date = new DateTime(2020, 1, 3), Amount = 300, Status = PaymentStatus.Closed });

            _paymentData.Setup(p => p.Commit()).ReturnsAsync(1); // mock successful db save of 1 record
        }

        [Test]
        public async Task CancelPayment_WhenPending_CancelsPayment()
        {
            var sut = new PaymentService(_paymentData.Object, _accountData.Object);

            var result = await sut.CancelPaymentRequest(1);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(PaymentStatus.Closed, result.Entity.Status);
        }

        [Test]
        public async Task CancelPayment_WhenPending_CancelsPaymentWithReason()
        {
            var sut = new PaymentService(_paymentData.Object, _accountData.Object);

            var result = await sut.CancelPaymentRequest(1, "Didn't like the look of it");

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual("Didn't like the look of it", result.Entity.ClosedReason);
            Assert.AreEqual(PaymentStatus.Closed, result.Entity.Status);
        }

        [Test]
        public async Task CancelPayment_WhenProcessed_Fails()
        {
            var sut = new PaymentService(_paymentData.Object, _accountData.Object);

            var result = await sut.CancelPaymentRequest(2);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(PaymentStatus.Processed, result.Entity.Status);
        }

        [Test]
        public async Task CancelPayment_WhenClosed_Fails()
        {
            var sut = new PaymentService(_paymentData.Object, _accountData.Object);

            var result = await sut.CancelPaymentRequest(3);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(PaymentStatus.Closed, result.Entity.Status);
        }
    }
}