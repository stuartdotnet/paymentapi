using PaymentAPI.Data;
using PaymentAPI.Model;
using System.Collections.Generic;

namespace PaymentAPI.IntegrationTests
{
    public static class Seeding
    {
        public static void InitializeDbForTests(PaymentsContext db)
        {
            db.Accounts.AddRange(GetAccounts());
            db.PaymentRequests.AddRange(GetPaymentRequests());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(PaymentsContext db)
        {
            db.Accounts.RemoveRange(db.Accounts);
            db.PaymentRequests.RemoveRange(db.PaymentRequests);
            InitializeDbForTests(db);
        }

        public static List<PaymentRequest> GetPaymentRequests()
        {
            return new List<PaymentRequest>()
            {
                new PaymentRequest { AccountId = 1, Amount = 100, Status = PaymentStatus.Pending },
                new PaymentRequest { AccountId = 2, Amount = 100, Status = PaymentStatus.Closed },
                new PaymentRequest { AccountId = 3, Amount = 100, Status = PaymentStatus.Pending },
                new PaymentRequest { AccountId = 4, Amount = 100, Status = PaymentStatus.Pending },
            };
        }

        public static List<Account> GetAccounts()
        {
            return new List<Account>()
            {
                new Account { AccountId = 1, Balance = 200 },
                new Account { AccountId = 2, Balance = 200 },
                new Account { AccountId = 3, Balance = 200 },
                new Account { AccountId = 4, Balance = 200 },
            };
        }
    }
}
