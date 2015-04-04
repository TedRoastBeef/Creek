using System;

namespace Creek.UI.EFML.Base.Exceptions
{
    public class EfmlException : Exception
    {
        public EfmlException(string msg)
            : base(msg)
        {
        }
    }
}