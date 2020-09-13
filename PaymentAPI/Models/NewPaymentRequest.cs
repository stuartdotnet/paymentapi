using System;
using System.ComponentModel.DataAnnotations;
using PaymentAPI.Business.Validation;

namespace PaymentAPI.Models
{
    /// <summary>
    /// A Payment Request object
    /// </summary>
    public class NewPaymentRequest
    {
        /// <summary>
        /// Monetary value of the request
        /// </summary>
        [Required]
        public decimal? Amount { get; set; }

        /// <summary>
        /// Date payment request occurs
        /// </summary>
        [Required]
        [RequiredDate]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Account Id for the customer of whom to apply the payment
        /// </summary>
        [Required]
        public int? AccountId { get; set; }
    }

}
