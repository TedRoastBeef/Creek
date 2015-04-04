namespace Creek.IO.Binary.BinaryTypes
{
    class PackageBinary : Binary<Package>
    {

        public override void OnWrite(Writer bw, Package value)
        {
            var c = value;
            bw.Write(c.Tag);
            bw.Write(c.CreationTime);
            bw.WriteArray(c.RawData);
        }

        public override Package OnRead(Reader br)
        {
            return new Package {Tag = br.Read<string>().To<string>(), CreationTime = br.Read<System.DateTime>().To<System.DateTime>(), RawData = br.ReadArray<byte>()};
        }
    }
}
