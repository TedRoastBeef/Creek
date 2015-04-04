using System;

namespace Creek.Scripting.Commandparser.Exceptions
{
    public class FatalError : Exception
    {

        public FatalError(string m)
            :base(m)
        {
        }

    }
}
