using PaymentAPI.Data;
using PaymentAPI.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Business
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentData _paymentsData;
        private readonly IAccountData _accountData;

        public PaymentService(IPaymentData paymentData, IAccountData accountData)
        {
            this._paymentsData = paymentData;
            this._accountData = accountData;
        }

        public async Task<Result<PaymentRequest>> CreatePaymentRequest(PaymentRequest paymentRequest)
        {
            if (paymentRequest.Amount <= 0) return new Result<PaymentRequest> { Success = false, Message = "Payment must be greater than zero." };

            var balanceResult = await _accountData.GetBalance(paymentRequest.AccountId);
            if (!balanceResult.Success) return new Result<PaymentRequest> { Success = false, Message = balanceResult.Message };

            if (paymentRequest.Amount > balanceResult.Entity)
            {
                paymentRequest.Status = PaymentStatus.Closed;
                paymentRequest.ClosedReason = PaymentConstants.NotEnoughFunds;

                await _paymentsData.Add(paymentRequest);

                if (await _paymentsData.Commit() > 0)
                {
                    return new Result<PaymentRequest> { Entity = paymentRequest, Message = paymentRequest.ClosedReason, Success = false };
                }
                
                return new Result<PaymentRequest> { Entity = paymentRequest, Message = PaymentConstants.CouldNotBeSaved, Success = false };
            }

            paymentRequest.Status = PaymentStatus.Pending;

            await _paymentsData.Add(paymentRequest);
            await _paymentsData.Commit();

            return new Result<PaymentRequest> { Entity = paymentRequest, Success = true, Message = "Payment request created successfully." };
        }

        public async Task<Result<PaymentRequest>> CancelPaymentRequest(int paymentRequestId, string reason = null)
        {
            var paymentRequest = await _paymentsData.Get(paymentRequestId);

            if (paymentRequest == null) return new Result<PaymentRequest> { Message = PaymentConstants.NotFound, Success = false };

            if (paymentRequest.Status != PaymentStatus.Pending)
            {
                return new Result<PaymentRequest> { Entity = paymentRequest, Message = $"Cannot cancel payment because it is {paymentRequest.Status}", Success = false };
            }

            ClosePaymentRequest(paymentRequest, reason);

            if (await _paymentsData.Commit() > 0)
            {
                return new Result<PaymentRequest> { Entity = paymentRequest, Message = "Payment request cancelled successfully.", Success = true };
            }

            return new Result<PaymentRequest> { Entity = paymentRequest, Message = PaymentConstants.CouldNotBeSaved, Success = false };
        }

        public async Task<Result<PaymentRequest>> ProcessPaymentRequest(int paymentRequestId)
        {
            var paymentRequest = await _paymentsData.Get(paymentRequestId);

            if (paymentRequest == null) return new Result<PaymentRequest> { Message = PaymentConstants.NotFound, Success = false };

            if (paymentRequest.Status != PaymentStatus.Pending)
            {
                return new Result<PaymentRequest> { Entity = paymentRequest, Message = $"Cannot process payment because it is {paymentRequest.Status}.", Success = false };
            }

            if (paymentRequest.Amount > paymentRequest.Account.Balance)
            {
                ClosePaymentRequest(paymentRequest, PaymentConstants.NotEnoughFunds);
                if (await _paymentsData.Commit() > 0)
                {
                    return new Result<PaymentRequest> { Entity = paymentRequest, Message = PaymentConstants.NotEnoughFunds, Success = false };
                }
            }
            else
            {
                paymentRequest.Status = PaymentStatus.Processed;
                paymentRequest.Account.Balance -= paymentRequest.Amount;

                _paymentsData.Update(paymentRequest);

                if (await _paymentsData.Commit() > 0)
                {
                    return new Result<PaymentRequest> { Entity = paymentRequest, Message = "Payment request processed successfully.", Success = true };
                }
            }

            return new Result<PaymentRequest> { Entity = paymentRequest, Message = PaymentConstants.CouldNotBeSaved, Success = false };
        }

        public IEnumerable<PaymentRequest> GetPaymentRequests(int accountId)
        {
            return _paymentsData.GetPaymentRequestsForAccount(accountId).OrderByDescending(p => p.Date);
        }

        private void ClosePaymentRequest(PaymentRequest paymentRequest, string reason)
        {
            paymentRequest.Status = PaymentStatus.Closed;
            paymentRequest.ClosedReason = reason;

            _paymentsData.Update(paymentRequest);
        }
    }
}
