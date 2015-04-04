using System;

namespace Creek.UI.EFML.Base.Exceptions
{
    public class JsException : Exception
    {
        public JsException(string msg)
            : base(msg)
        {
        }
    }
}