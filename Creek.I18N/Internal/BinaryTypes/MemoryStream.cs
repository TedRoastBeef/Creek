namespace Creek.I18N.Internal.BinaryTypes
{
    class MemoryStream : Creek.I18N.Internal.Binary
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
