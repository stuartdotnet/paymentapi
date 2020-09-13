using PaymentAPI.Model;
using System.Threading.Tasks;

namespace PaymentAPI.Data
{
    public interface IAccountData
    {
        Task<Result<decimal>> GetBalance(int accountId);
    }
}
