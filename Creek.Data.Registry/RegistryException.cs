using System;

namespace Creek.Data.Registry
{
    public class RegistryException : ApplicationException
    {
        public RegistryException(string message)
            : base(message)
        {
        }

        public RegistryException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}