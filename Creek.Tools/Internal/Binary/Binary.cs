using System;
using Lib.IO.Binary;

namespace Lib.IO.Internal.Binary
{
    internal class Binary
    {
        public Action<Writer, object> OnWrite;
        public Func<Reader, object> OnRead;

        public Binary(Action<Writer, object> write, Func<Reader, object> read)
        {
            OnWrite = write;
            OnRead = read;
        }
        public Binary()
        {
            
        }

    }
}