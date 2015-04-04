
using System;

namespace Creek.Parsing.Tokenizer
{
    public class TokenizerException : Exception
    {
        public TokenizerException(string message)
            : base(message) { }
    }
}
