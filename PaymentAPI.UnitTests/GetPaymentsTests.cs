using Moq;
using NUnit.Framework;
using PaymentAPI.Business;
using PaymentAPI.Data;
using PaymentAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentAPI.UnitTests
{
    public class GetPaymentsTests
    {
        private Mock<IPaymentData> _paymentData;
        private Mock<IAccountData> _accountData;

        private List<PaymentRequest> _paymentRequestsAccount1;
        private List<PaymentRequest> _paymentRequestsAccount2;

        [SetUp]
        public void Setup()
        {
            _paymentData = new Mock<IPaymentData>();
            _accountData = new Mock<IAccountData>();

            _paymentRequestsAccount1 = new List<PaymentRequest>
            {
                new PaymentRequest { AccountId = 1, Date = new DateTime(2020, 1, 3), Amount = 100, Status = PaymentStatus.Pending },
                new PaymentRequest { AccountId = 1, Date = new DateTime(2020, 1, 2), Amount = 200, Status = PaymentStatus.Processed },
                new PaymentRequest { AccountId = 1, Date = new DateTime(2020, 1, 1), Amount = 300, Status = PaymentStatus.Processed }
            };

            _paymentRequestsAccount2 = new List<PaymentRequest>
            {
                new PaymentRequest { AccountId = 2, Date = new DateTime(2020, 1, 4), Amount = 400, Status = PaymentStatus.Closed, ClosedReason = PaymentConstants.NotEnoughFunds }
            };

            _paymentData.Setup(p => p.GetPaymentRequestsForAccount(1)).Returns(_paymentRequestsAccount1);
            _paymentData.Setup(p => p.GetPaymentRequestsForAccount(2)).Returns(_paymentRequestsAccount2);
        }

        [Test]
        public void WhenPaymentsForAccountRequested_ReturnsCorrectAccountPayments()
        {
            var sut = new PaymentService(_paymentData.Object, _accountData.Object);

            var result = sut.GetPaymentRequests(1).ToList();

            Assert.IsTrue(result.All(p => p.AccountId == 1));
        }

        [Test]
        public void WhenPaymentsRequested_ReturnNewestPaymentsFirst()
        {
            var sut = new PaymentService(_paymentData.Object, _accountData.Object);

            var result = sut.GetPaymentRequests(1).ToList();

            Assert.AreEqual(100, result[0].Amount);
            Assert.AreEqual(200, result[1].Amount);
            Assert.AreEqual(300, result[2].Amount);
        }

        [Test]
        public void WhenClosedPaymentsRequested_ReturnsClosedReason()
        {
            var sut = new PaymentService(_paymentData.Object, _accountData.Object);

            var result = sut.GetPaymentRequests(2).ToList();

            Assert.AreEqual(PaymentConstants.NotEnoughFunds, result[0].ClosedReason);
        }
    }
}