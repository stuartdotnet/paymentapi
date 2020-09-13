using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentAPI.Business.Validation
{
    public class RequiredDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime && (DateTime)value != DateTime.MinValue)
                    return ValidationResult.Success;

            return new ValidationResult("Date is required.");
        }
    }
}
