namespace Creek.Validation
{
    using Creek.Validation.Attributes;

    public class CreditCardNumberAttribute : RegExAttribute
    {
        public CreditCardNumberAttribute()
            : base(@"^(?:4[0-9]{12}(?:[0-9]{3})?  # Visa|  5[1-5][0-9]{14}  # MasterCard|  3[47][0-9]{13}  # American Express|  3(?:0[0-5]|[68][0-9])[0-9]{11}   # Diners Club|  6(?:011|5[0-9]{2})[0-9]{12}      # Discover|  (?:2131|1800|35\d{3})\d{11}      # JCB)$", "{0} must be a valid Credit Card Number")
        {
        }
    }
}