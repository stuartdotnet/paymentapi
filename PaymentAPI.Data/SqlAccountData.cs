using Microsoft.EntityFrameworkCore;
using PaymentAPI.Model;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Data
{
    public class SqlAccountData : IAccountData
    {
        private readonly PaymentsContext _context;

        public SqlAccountData(PaymentsContext context)
        {
            _context = context;
        }

        public async Task<Result<decimal>> GetBalance(int accountId)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == accountId);

            if (account == null)
                return new Result<decimal> { Success = false, Message = $"Account not found for account id {accountId}." };

            return new Result<decimal> { Entity = account.Balance, Success = true };
        }
    }
}
