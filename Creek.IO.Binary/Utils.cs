using Creek.IO.Binary.BinaryTypes;

namespace Creek.IO.Binary
{
    public class Utils
    {
        public static TypeBinaryDict InitTypes()
        {
            var r = new TypeBinaryDict();

            r.AddC(new PrimBinary<long>((bw, value) => bw.Write(value), (br) => br.ReadInt64()));
            r.AddC(new PrimBinary<byte>((bw, value) => bw.Write(value), (br) => br.ReadByte()));
            r.AddC(new PrimBinary<sbyte>((bw, value) => bw.Write(value), (br) => br.ReadSByte()));
            r.AddC(new PrimBinary<bool>((bw, value) => bw.Write(value), (br) => br.ReadBoolean()));
            r.AddC(new PrimBinary<uint>((bw, value) => bw.Write(value), (br) => br.ReadUInt32()));
            r.AddC(new PrimBinary<int>((bw, value) => bw.Write(value), (br) => br.ReadInt32()));
            r.AddC(new PrimBinary<char>((bw, value) => bw.Write(value), (br) => br.ReadChar()));
            r.AddC(new PrimBinary<decimal>((bw, value) => bw.Write(value), (br) => br.ReadDecimal()));
            r.AddC(new PrimBinary<double>((bw, value) => bw.Write(value), (br) => br.ReadDouble()));
            r.AddC(new PrimBinary<float>((bw, value) => bw.Write(value), (br) => br.ReadSingle()));
            r.AddC(new PrimBinary<string>((bw, value) => bw.Write(value), (br) => br.ReadString()));

            r.AddC(new Color());
            r.AddC(new Point());
            r.AddC(new Size());
            r.AddC(new BinaryTypes.DateTime());
            r.AddC(new MemoryStream());
            r.AddC(new Image());

            r.AddC(new PackageBinary());

            return r;
        }
    }
}
