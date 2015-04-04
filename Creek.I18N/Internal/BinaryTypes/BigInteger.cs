namespace Creek.I18N.Internal.BinaryTypes
{
    class BigInteger : Creek.I18N.Internal.Binary
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
