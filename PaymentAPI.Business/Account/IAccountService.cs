using PaymentAPI.Model;
using System.Threading.Tasks;

namespace PaymentAPI.Business
{
    public interface IAccountService
    {
        Task<Result<decimal>> GetBalance(int customerId);
    }
}