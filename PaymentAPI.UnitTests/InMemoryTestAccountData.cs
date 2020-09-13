using PaymentAPI.Data;
using PaymentAPI.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.UnitTests
{
    public class InMemoryTestAccountData : IAccountData
    {
        public IEnumerable<Account> Accounts { get; set; }

        public async Task<Result<decimal>> GetBalance(int accountId)
        {
            var account = await Task.Run(() =>  Accounts.SingleOrDefault(a => a.AccountId == accountId));

            if (account == null) 
                return new Result<decimal> { Success = false, Message = $"Account not found for account id {accountId}." };

            return new Result<decimal> { Entity = account.Balance, Success = true };
        }
    }
}
