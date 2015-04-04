using System.Drawing.Imaging;
using System.IO;
using Lib.IO.Internal.Binary;

namespace Lib.IO.Binary.BinaryTypes
{
    class Image : Internal.Binary.Binary
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
