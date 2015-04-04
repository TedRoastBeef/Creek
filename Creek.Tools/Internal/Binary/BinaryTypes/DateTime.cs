using System.IO;

namespace Lib.IO.Binary.BinaryTypes
{
    class DateTime : Internal.Binary.Binary
    {

        public DateTime()
        {
            OnRead = OnReads;
            OnWrite = OnWrites;
        }

        private void OnWrites(BinaryWriter bw, object value)
        {
            var p = (System.DateTime)value;
            bw.Write(p.ToString());
        }

        private object OnReads(BinaryReader br)
        {
            return System.DateTime.Parse(br.ReadString());
        }
    }
}
