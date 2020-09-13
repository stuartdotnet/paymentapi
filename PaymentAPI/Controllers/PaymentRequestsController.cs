using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Business;
using PaymentAPI.Model;
using PaymentAPI.Models;

namespace PaymentAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentRequestsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public PaymentRequestsController(IPaymentService paymentService, IAccountService accountService, IMapper mapper)
        {
            _paymentService = paymentService;
            _accountService = accountService;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new Payment Request
        /// </summary>
        /// <param name="paymentRequestDto">A Payment Request object</param>
        /// <returns>201 status if successful</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> CreatePaymentRequest(NewPaymentRequest paymentRequestDto)
        {
            var paymentRequest = _mapper.Map<PaymentRequest>(paymentRequestDto);

            Result<PaymentRequest> result = await _paymentService.CreatePaymentRequest(paymentRequest);

            if (result.Success)
            {
                return StatusCode(201);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Cancels a payment request
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="reason">Optionally provide a reason for the cancellation</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{paymentRequestId}")]
        public async Task<IActionResult> CancelPaymentRequest(int paymentRequestId, [FromQuery] string reason)
        {
            var result = await _paymentService.CancelPaymentRequest(paymentRequestId, reason);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Processes a payment request
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{paymentRequestId}")]
        public async Task<IActionResult> ProcessPaymentRequest(int paymentRequestId)
        {
            var result = await _paymentService.ProcessPaymentRequest(paymentRequestId);

            if (result.Success)
            {
                return Ok(_mapper.Map<NewPaymentRequest>(result.Entity));
            }

            return BadRequest(result.Message);
        }


        /// <summary>
        /// Gets the Balance and Payments associated with an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>The account balance and a list of payment requests</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetBalanceAndPayments(int accountId)
        {
            var balanceResult = await _accountService.GetBalance(accountId);

            if (!balanceResult.Success)
                return NotFound(balanceResult.Message);

            var payments = _paymentService.GetPaymentRequests(accountId);

            return Ok(new BalanceAndPaymentRequests()
            {
                Balance = balanceResult.Entity,
                PaymentRequests = payments.Select(p => _mapper.Map<PaymentRequestItem>(p))
            });
        }
    }
}
