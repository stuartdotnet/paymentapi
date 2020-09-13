using Newtonsoft.Json;
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
        private const string CONTROLLERURL = "api/PaymentRequests";

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _factory = new APIWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }

        // fix
        [Test]
        public async Task WhenPaymentRequestPosted_ThenTheResultIsCreated()
        {
            NewPaymentRequest paymentRequest = new NewPaymentRequest()
            {
                AccountId = 1,
                Date = new DateTime(2020, 1, 1),
                Amount = 50
            };
            var content = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync(CONTROLLERURL, content);
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode, result.ReasonPhrase);
        }

        [Test]
        public async Task WhenBadPaymentRequestPosted_ThenTheResultIsBadRequest()
        {
            NewPaymentRequest paymentRequest = new NewPaymentRequest()
            {
                AccountId = 1,
                Date = new DateTime(2020, 1, 1),
                Amount = 0
            };

            var content = new StringContent(paymentRequest.ToString(), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync(CONTROLLERURL, content);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
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
            var paymentRequest = new { paymentrequestId = 3 };
            var result = await _client.PostAsync(CONTROLLERURL + "/3", new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json"));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task WhenClosedPaymentRequestProcessed_ThenTheResultIsBadRequest()
        {
            var paymentRequest = new { paymentrequestId = 2 }; // closed payment
            var result = await _client.PostAsync(CONTROLLERURL + "/2", new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json"));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GetPaymentRequestForNoAccount_ThenTheResultIsNotFound()
        {
            const string ACCOUNTID = "5";
            var result = await _client.GetAsync(CONTROLLERURL + "/" + ACCOUNTID);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetPaymentRequestForValidAccount_ThenTheResultIsFound()
        {
            const string ACCOUNTID = "1";
            var result = await _client.GetAsync(CONTROLLERURL + "/" + ACCOUNTID);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
