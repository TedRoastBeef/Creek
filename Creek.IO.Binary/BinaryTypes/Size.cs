namespace Creek.IO.Binary.BinaryTypes
{
    class Size : Binary<System.Drawing.Size>
    {

        public override void OnWrite(Writer bw, System.Drawing.Size value)
        {
            var p = value;
            bw.Write(p.Height); bw.Write(p.Width);
        }
        public override System.Drawing.Size OnRead(Reader br)
        {
            return new System.Drawing.Size(br.Read<int>(), br.Read<int>());
        }
    }
}
