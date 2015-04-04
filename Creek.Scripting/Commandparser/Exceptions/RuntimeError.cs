using System;

namespace Creek.Scripting.Commandparser.Exceptions
{
    public class RuntimeError : Exception
    {

        public RuntimeError(string m)
            : base(m)
        {
        }

    }
}
