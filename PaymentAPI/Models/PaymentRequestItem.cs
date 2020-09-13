using System;
namespace PaymentAPI.Models
{
    /// <summary>
    /// A Payment Request object
    /// </summary>
    public class PaymentRequestItem
    {
        /// <summary>
        /// Monetary value of the request
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Date payment request occurs
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Status of the Payment
        /// </summary>
        public string PaymentStatus { get; set; }

        /// <summary>
        /// Reason payment was cancelled
        /// </summary>
        public string ClosedReason { get; set; }
    }

}
