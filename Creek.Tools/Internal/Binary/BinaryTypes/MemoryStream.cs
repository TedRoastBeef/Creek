using System.IO;
using Lib.IO.Internal.Binary;

namespace Lib.IO.Binary.BinaryTypes
{
    class MemoryStream : Internal.Binary.Binary
    {

        public MemoryStream()
        {
            OnRead = OnReads;
            OnWrite = OnWrites;
        }

        private void OnWrites(Writer bw, object value)
        {
            bw.Write<byte>(((System.IO.MemoryStream)value).ToArray(), true);
        }

        private object OnReads(Reader br)
        {
            return new System.IO.MemoryStream(br.Read<byte>(true).To<byte[]>());
        }
    }
}
