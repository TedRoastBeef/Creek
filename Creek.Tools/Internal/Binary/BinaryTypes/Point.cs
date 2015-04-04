using System.IO;

namespace Lib.IO.Binary.BinaryTypes
{
    class Point : Internal.Binary.Binary
    {

        public Point()
        {
            OnRead = OnReads;
            OnWrite = OnWrites;
        }

        private void OnWrites(BinaryWriter bw, object value)
        {
            var p = (System.Drawing.Point)value;
            bw.Write(p.X); bw.Write(p.Y);
        }

        private object OnReads(BinaryReader br)
        {
            return new System.Drawing.Point(br.ReadInt32(), br.ReadInt32());
        }
    }
}
