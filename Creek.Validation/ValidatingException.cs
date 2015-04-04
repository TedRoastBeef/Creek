namespace Creek.Validation
{
    using System;

    public class ValidatingException : Exception
    {
        public ValidatingException(string msg)
            : base(msg)
        {
        }
    }
}