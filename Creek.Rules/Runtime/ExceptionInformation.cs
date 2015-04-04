using System;

namespace Creek.Rules.Runtime
{
    public class ExceptionInformation
    {
        public Exception Exception { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }
    }
}
