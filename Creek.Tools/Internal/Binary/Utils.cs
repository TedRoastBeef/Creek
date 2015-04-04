using System;
using Lib.IO.Binary.BinaryTypes;
using DateTime = System.DateTime;

namespace Lib.IO.Internal.Binary
{
    internal class Utils
    {
        internal static TypeBinaryDict InitTypes()
        {
            var r = new TypeBinaryDict();

            r.AddC(typeof(long), new Internal.Binary.Binary((bw, value) => bw.Write((long)value), (br) => br.ReadInt64()));
            r.AddC(typeof(byte), new Internal.Binary.Binary((bw, value) => bw.Write((byte)value), (br) => br.ReadByte()));
            r.AddC(typeof(sbyte), new Internal.Binary.Binary((bw, value) => bw.Write((sbyte)value), (br) => br.ReadSByte()));
            r.AddC(typeof(bool), new Internal.Binary.Binary((bw, value) => bw.Write((bool)value), (br) => br.ReadBoolean()));
            r.AddC(typeof(uint), new Internal.Binary.Binary((bw, value) => bw.Write((uint)value), (br) => br.ReadUInt32()));
            r.AddC(typeof(UInt16), new Internal.Binary.Binary((bw, value) => bw.Write((UInt16)value), (br) => br.ReadUInt16()));
            r.AddC(typeof(UInt64), new Internal.Binary.Binary((bw, value) => bw.Write((UInt64)value), (br) => br.ReadUInt64()));
            r.AddC(typeof(int), new Internal.Binary.Binary((bw, value) => bw.Write((int)value), (br) => br.ReadInt32()));
            r.AddC(typeof(Int16), new Internal.Binary.Binary((bw, value) => bw.Write((Int16)value), (br) => br.ReadInt16()));
            r.AddC(typeof(Int64), new Internal.Binary.Binary((bw, value) => bw.Write((Int64)value), (br) => br.ReadInt64()));
            r.AddC(typeof(char), new Internal.Binary.Binary((bw, value) => bw.Write((char)value), (br) => br.ReadChar()));
            r.AddC(typeof(decimal), new Internal.Binary.Binary((bw, value) => bw.Write((decimal)value), (br) => br.ReadDecimal()));
            r.AddC(typeof(double), new Internal.Binary.Binary((bw, value) => bw.Write((double)value), (br) => br.ReadDouble()));
            r.AddC(typeof(float), new Internal.Binary.Binary((bw, value) => bw.Write((float)value), (br) => br.ReadSingle()));
            r.AddC(typeof(string), new Internal.Binary.Binary((bw, value) => bw.Write((string)value), (br) => br.ReadString()));

            r.AddC(typeof(Color), new IO.Binary.BinaryTypes.Color());
            r.AddC(typeof(Point), new IO.Binary.BinaryTypes.Point());
            r.AddC(typeof(Size), new IO.Binary.BinaryTypes.Size());
            r.AddC(typeof(DateTime), new IO.Binary.BinaryTypes.DateTime());
            r.AddC(typeof(MemoryStream), new IO.Binary.BinaryTypes.MemoryStream());
            r.AddC(typeof(Image), new IO.Binary.BinaryTypes.Image());
            r.AddC(typeof(BigInteger), new IO.Binary.BinaryTypes.BigInteger());

            return r;
        }
    }
}
