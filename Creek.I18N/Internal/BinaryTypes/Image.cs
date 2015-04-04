using System.Drawing.Imaging;

namespace Creek.I18N.Internal.BinaryTypes
{
    class Image : Creek.I18N.Internal.Binary
    {

        public Image()
        {
            OnRead = OnReads;
            OnWrite = OnWrites;
        }

        private void OnWrites(Writer bw, object value)
        {
            var img = (System.Drawing.Image)value;
            var ms = new System.IO.MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            bw.Write(ms.ToArray());
        }

        private object OnReads(Reader br)
        {
            var ms = new System.IO.MemoryStream(br.Read<byte>(true).To<byte[]>());
            return System.Drawing.Image.FromStream(ms);
        }
    }
}
