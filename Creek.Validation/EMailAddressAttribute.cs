namespace Creek.Validation
{
    using Creek.Validation.Attributes;

    public class EMailAddressAttribute : RegExAttribute
    {
        public EMailAddressAttribute()
            : base(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", "{0} must be a valid E-Mail Address")
        {
        }
    }
}
