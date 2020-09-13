using Moq;
using NUnit.Framework;
using PaymentAPI.Business;
using PaymentAPI.Data;
using PaymentAPI.Model;
using System.Threading.Tasks;

namespace PaymentAPI.UnitTests
{
    public class CreatePaymentRequestTests
    {
        private Mock<IPaymentData> _paymentData;
        private Mock<IAccountData> _accountData;

        [SetUp]
        public void Setup()
        {
            _paymentData = new Mock<IPaymentData>();
            _accountData = new Mock<IAccountData>();
        }

        [Test]
        public async Task WhenAmountLessThanBalance_PaymentSavedAsPending()
        {
            var paymentRequest = new PaymentRequest() { Amount = 100 };

            _paymentData.Setup(p => p.Add(It.IsAny<PaymentRequest>())).ReturnsAsync(paymentRequest);
            _accountData.Setup(p => p.GetBalance(It.IsAny<int>())).ReturnsAsync(new Result<decimal> { Success = true, Entity = 200 });

            var sut = new PaymentService(_paymentData.Object, _accountData.Object);

            var result = await sut.CreatePaymentRequest(paymentRequest);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(PaymentStatus.Pending, result.Entity.Status);
        }

        [Test]
        public async Task WhenAmountMoreThanBalance_PaymentSavedAsClosedWithReason()
        {
            var paymentRequest = new PaymentRequest() { Amount = 300 };

            _paymentData.Setup(p => p.Add(It.IsAny<PaymentRequest>())).ReturnsAsync(paymentRequest);
            _accountData.Setup(p => p.GetBalance(It.IsAny<int>())).ReturnsAsync(new Result<decimal> { Success = true, Entity = 200 });

            var sut = new PaymentService(_paymentData.Object, _accountData.Object);

            var result = await sut.CreatePaymentRequest(paymentRequest);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(PaymentStatus.Closed, result.Entity.Status);
            Assert.AreEqual(PaymentConstants.NotEnoughFunds, result.Entity.ClosedReason);
        }
    }
}