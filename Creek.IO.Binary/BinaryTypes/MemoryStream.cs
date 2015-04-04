namespace Creek.IO.Binary.BinaryTypes
{
    class MemoryStream : Binary<System.IO.MemoryStream>
    {

        public override void OnWrite(Writer bw, System.IO.MemoryStream value)
        {
            bw.WriteArray((value).ToArray());
        }

        public override System.IO.MemoryStream OnRead(Reader br)
        {
            return new System.IO.MemoryStream(br.ReadArray<byte>());
        }
    }
}
