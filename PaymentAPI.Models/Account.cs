using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentAPI.Model
{
    public class Account
    {
        public int AccountId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
