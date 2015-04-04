using System.IO;

namespace Creek.I18N.Internal.BinaryTypes
{
    class DateTime : Creek.I18N.Internal.Binary
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
