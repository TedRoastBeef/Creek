using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Creek.I18N.Internal.Types;

namespace Creek.I18N.Internal
{
    internal class Utils
    {
        internal static TypeBinaryDict InitTypes()
        {
            var r = new TypeBinaryDict();

            r.AddC(typeof(long), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((long)value), (br) => br.ReadInt64()));
            r.AddC(typeof(byte), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((byte)value), (br) => br.ReadByte()));
            r.AddC(typeof(sbyte), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((sbyte)value), (br) => br.ReadSByte()));
            r.AddC(typeof(bool), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((bool)value), (br) => br.ReadBoolean()));
            r.AddC(typeof(uint), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((uint)value), (br) => br.ReadUInt32()));
            r.AddC(typeof(UInt16), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((UInt16)value), (br) => br.ReadUInt16()));
            r.AddC(typeof(UInt64), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((UInt64)value), (br) => br.ReadUInt64()));
            r.AddC(typeof(int), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((int)value), (br) => br.ReadInt32()));
            r.AddC(typeof(Int16), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((Int16)value), (br) => br.ReadInt16()));
            r.AddC(typeof(Int64), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((Int64)value), (br) => br.ReadInt64()));
            r.AddC(typeof(char), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((char)value), (br) => br.ReadChar()));
            r.AddC(typeof(decimal), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((decimal)value), (br) => br.ReadDecimal()));
            r.AddC(typeof(double), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((double)value), (br) => br.ReadDouble()));
            r.AddC(typeof(float), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((float)value), (br) => br.ReadSingle()));
            r.AddC(typeof(string), new Creek.I18N.Internal.Binary((bw, value) => bw.Write((string)value), (br) => br.ReadString()));

            r.AddC(typeof(Color), new BinaryTypes.Color());
            r.AddC(typeof(Point), new BinaryTypes.Point());
            r.AddC(typeof(Size), new BinaryTypes.Size());
            r.AddC(typeof(DateTime), new BinaryTypes.DateTime());
            r.AddC(typeof(MemoryStream), new BinaryTypes.MemoryStream());
            r.AddC(typeof(Image), new BinaryTypes.Image());
            r.AddC(typeof(BigInteger), new BinaryTypes.BigInteger());
            r.AddC(typeof(Dictionary<string, string>), new BinaryTypes.Dictionary<string, string>());

            return r;
        }
    }
}
