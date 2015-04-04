namespace Creek.Validation
{
    using Creek.Validation.Attributes;

    public class PhoneNumberAttribute : RegExAttribute
    {
        public PhoneNumberAttribute()
            : base(@"^\(\d{1,2}(\s\d{1,2}){1,2}\)\s(\d{1,2}(\s\d{1,2}){1,2})((-(\d{1,4})){0,1})$", "{0} must be a valid Phone Number") //ToDo: implement phone pattern
        {
        }
    }
}
