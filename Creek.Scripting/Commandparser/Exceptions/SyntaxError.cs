using System;

namespace Creek.Scripting.Commandparser.Exceptions
{
    public class SyntaxError : Exception
    {

        public SyntaxError(string m)
            : base(m)
        {
        }

    }
}
