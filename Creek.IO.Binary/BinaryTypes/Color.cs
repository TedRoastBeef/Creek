namespace Creek.IO.Binary.BinaryTypes
{
    class Color : Binary<System.Drawing.Color>
    {

        public override void OnWrite(Writer bw, System.Drawing.Color value)
        {
            var c = value; bw.Write(c.A);
            bw.Write(c.R); bw.Write(c.G); bw.Write(c.B); 
        }

        public override System.Drawing.Color OnRead(Reader br)
        {
            return System.Drawing.Color.FromArgb(br.Read<byte>(), br.Read<byte>(), br.Read<byte>(),
                                                 br.Read<byte>());
        }
    }
}
