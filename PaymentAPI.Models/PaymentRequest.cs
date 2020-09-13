using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentAPI.Model
{
    public class PaymentRequest
    {
        public int PaymentRequestId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public PaymentStatus Status { get; set; }
        public string ClosedReason { get; set; }


        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
