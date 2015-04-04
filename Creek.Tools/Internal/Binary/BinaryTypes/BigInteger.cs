using System.IO;
using Lib.IO.Internal.Binary;

namespace Lib.IO.Binary.BinaryTypes
{
    class BigInteger : Internal.Binary.Binary
    {

        public BigInteger()
        {
            OnRead = OnReads;
            OnWrite = OnWrites;
        }

        private void OnWrites(Writer bw, object value)
        {
            var bi = (Types.BigInteger)value;
            bw.Write(bi.LongValue());
        }

        private object OnReads(Reader br)
        {
            return new Types.BigInteger(br.Read<long>().To<long>());
        }
    }
}
