using PaymentAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentAPI.Business
{
    public interface IPaymentService
    {
        Task<Result<PaymentRequest>> CancelPaymentRequest(int paymentRequestId, string reason = null);
        Task<Result<PaymentRequest>> CreatePaymentRequest(PaymentRequest paymentRequest);
        Task<Result<PaymentRequest>> ProcessPaymentRequest(int paymentRequestId);
        IEnumerable<PaymentRequest> GetPaymentRequests(int accountId);
    }
}