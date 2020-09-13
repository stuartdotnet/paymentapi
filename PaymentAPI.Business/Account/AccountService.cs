using PaymentAPI.Data;
using PaymentAPI.Model;
using System.Threading.Tasks;

namespace PaymentAPI.Business
{
    public class AccountService : IAccountService
    {
        private readonly IAccountData _accountData;

        public AccountService(IAccountData accountData)
        {
            this._accountData = accountData;
        }

        public async Task<Result<decimal>> GetBalance(int accountId)
        {
            var result = await _accountData.GetBalance(accountId);
            if (result.Success)
                return new Result<decimal> { Entity = result.Entity, Success = true };
            else
                return new Result<decimal> { Success = false, Message = result.Message };
        }
    }
}
