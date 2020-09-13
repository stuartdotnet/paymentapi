using System.Collections.Generic;

namespace PaymentAPI.Models
{
    public class BalanceAndPaymentRequests
    {
        public decimal Balance { get; set; }
        public IEnumerable<PaymentRequestItem> PaymentRequests { get; set; }
    }
}
