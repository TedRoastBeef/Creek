namespace Creek.IO.Binary.BinaryTypes
{
    class Point : Binary<System.Drawing.Point>
    {

        public override void OnWrite(Writer bw, System.Drawing.Point value)
        {
            var p = value;
            bw.Write(p.X); bw.Write(p.Y);
        }

        public override System.Drawing.Point OnRead(Reader br)
        {
            return new System.Drawing.Point(br.Read<int>(), br.Read<int>());
        }
    }
}
