using System.IO;

namespace Lib.IO.Binary.BinaryTypes
{
    class Size : Internal.Binary.Binary
    {

        public Size()
        {
            OnRead = OnReads;
            OnWrite = OnWrites;
        }

        private void OnWrites(BinaryWriter bw, object value)
        {
            var p = (System.Drawing.Size)value;
            bw.Write(p.Height); bw.Write(p.Width);
        }

        private object OnReads(BinaryReader br)
        {
            return new System.Drawing.Size(br.ReadInt32(), br.ReadInt32());
        }
    }
}
