using Microsoft.EntityFrameworkCore;
using PaymentAPI.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Data
{
    public class SqlPaymentData : IPaymentData
    {
        private readonly PaymentsContext _context;

        public SqlPaymentData(PaymentsContext context)
        {
            _context = context;
        }

        public async Task<PaymentRequest> Add(PaymentRequest paymentRequest)
        {
            await _context.AddAsync(paymentRequest);
            return paymentRequest;
        }

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<PaymentRequest> Delete(int id)
        {
            var paymentRequest = await _context.FindAsync<PaymentRequest>(id);
            if (paymentRequest != null)
            {
                _context.PaymentRequests.Remove(paymentRequest);
            }
            return paymentRequest;
        }

        public async Task<PaymentRequest> Get(int id)
        {
            return await _context.PaymentRequests.Include(p => p.Account).SingleAsync(p => p.PaymentRequestId == id);
        }

        public IEnumerable<PaymentRequest> GetPaymentRequestsForAccount(int accountId)
        {
            return _context.PaymentRequests.Where(a => a.AccountId == accountId);
        }

        public PaymentRequest Update(PaymentRequest paymentRequest)
        {
            var entity = _context.PaymentRequests.Attach(paymentRequest);
            entity.State = EntityState.Modified;
            return paymentRequest;
        }
    }
}
