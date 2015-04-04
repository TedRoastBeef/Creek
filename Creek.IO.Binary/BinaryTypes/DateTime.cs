namespace Creek.IO.Binary.BinaryTypes
{
    class DateTime : Binary<System.DateTime>
    {
        public override void OnWrite(Writer bw, System.DateTime value)
        {
            var p = value;
            bw.Write(p.ToString());
        }

        public override System.DateTime OnRead(Reader br)
        {
            return System.DateTime.Parse(br.Read<string>());
        }
    }
}
