using PaymentAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentAPI.Data
{
    public interface IPaymentData
    {
        Task<PaymentRequest> Get(int id);
        IEnumerable<PaymentRequest> GetPaymentRequestsForAccount(int accountId);
        PaymentRequest Update(PaymentRequest paymentRequest);
        Task<PaymentRequest> Add(PaymentRequest paymentRequest);
        Task<PaymentRequest> Delete(int id); 
        Task<int> Commit();
    }
}
