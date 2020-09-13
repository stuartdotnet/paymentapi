using PaymentAPI.Model;
using System;
using System.Linq;

namespace PaymentAPI.Data
{
    public static class DbInitialiser
    {
        public static void Initialize(PaymentsContext context)
        { 
            context.Database.EnsureCreated();

            if (context.PaymentRequests.Any()) return; // DB already seeded

            var customers = new Customer[]
            {
                new Customer { FirstName = "Homer", LastName = "Simpson" },
                new Customer { FirstName = "Seymor", LastName = "Skinner" },
                new Customer { FirstName = "Barney", LastName = "Rubble" }
            };

            context.AddRange(customers);
            context.SaveChanges();

            var accounts = new Account[]
            {
                new Account { CustomerId = 1, Balance = 10000 },
                new Account { CustomerId = 2, Balance = 2000 },
                new Account { CustomerId = 3, Balance = 42 },
            };

            context.AddRange(accounts);
            context.SaveChanges();

            var payments = new PaymentRequest[]
            {
                    new PaymentRequest { Amount = 10, Date = new DateTime(2020, 1, 1), Status = PaymentStatus.Processed, AccountId = 3 },
                    new PaymentRequest { Amount = 2050, Date = new DateTime(2020, 2, 1), Status = PaymentStatus.Closed, ClosedReason = "no funds", AccountId = 2 },
                    new PaymentRequest { Amount = 3000, Date = new DateTime(2020, 3, 1), Status = PaymentStatus.Pending, AccountId = 1 },
                    new PaymentRequest { Amount = 100, Date = new DateTime(2020, 3, 2), Status = PaymentStatus.Pending, AccountId = 3 },
            };

            context.AddRange(payments);
            context.SaveChanges();
        }
    }
}

