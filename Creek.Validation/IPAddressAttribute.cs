namespace Creek.Validation
{
    using Creek.Validation.Attributes;

    public class IPAddressAttribute : RegExAttribute
    {
        public IPAddressAttribute()
            : base(@"\b(?:\d{1,3}\.){3}\d{1,3}\b", "{0} must be a valid IP-Address")
        {
        }
    }
}
