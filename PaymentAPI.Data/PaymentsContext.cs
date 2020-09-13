using Microsoft.EntityFrameworkCore;
using PaymentAPI.Model;
using System.Linq;

namespace PaymentAPI.Data
{
    public class PaymentsContext : DbContext
    {
        public PaymentsContext(DbContextOptions<PaymentsContext> options)
            : base(options)
        {
        }

        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}
