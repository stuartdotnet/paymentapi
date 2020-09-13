﻿namespace PaymentAPI.Model
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual Account Account { get; set; }
    }
}
