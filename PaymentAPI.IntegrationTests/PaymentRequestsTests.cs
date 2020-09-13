using NUnit.Framework;
using PaymentAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.IntegrationTests
{
    [TestFixture]
    public class PaymentRequestsControllerTests
    {
        private APIWebApplicationFactory<Startup> _factory;
        private HttpClient _client;
        private const string CONTROLLERURL = "/";

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _factory = new APIWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task WhenPaymentRequestPosted_ThenTheResultIsCreated()
        {
            NewPaymentRequest paymentRequest = new NewPaymentRequest()
            {
                AccountId = 1,
                Date = new DateTime(2020, 1, 1),
                Amount = 100
            };

            var content = new StringContent(paymentRequest.ToString(), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync(CONTROLLERURL, content);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public async Task WhenPaymentRequestCancelled_ThenTheResultIsOk()
        {
            const string ACCOUNTID = "1";
            var result = await _client.DeleteAsync(CONTROLLERURL + "/" + ACCOUNTID);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task WhenPaymentRequestProcessed_ThenTheResultIsOk()
        {
            const string PAYMENTREQUESTID = "1";
            var result = await _client.PostAsync(CONTROLLERURL, new StringContent(PAYMENTREQUESTID, Encoding.UTF8, "application/json"));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task WhenClosedPaymentRequestProcessed_ThenTheResultIsBadRequest()
        {
            const string PAYMENTREQUESTID = "2";
            var result = await _client.PostAsync(CONTROLLERURL, new StringContent(PAYMENTREQUESTID, Encoding.UTF8, "application/json"));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GetEmptyPaymentRequest_ThenTheResultIsNotFound()
        {
            var result = await _client.GetAsync(CONTROLLERURL);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
