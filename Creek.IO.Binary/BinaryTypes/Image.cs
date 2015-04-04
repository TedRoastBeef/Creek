using System.Drawing.Imaging;

namespace Creek.IO.Binary.BinaryTypes
{
    class Image : Binary<System.Drawing.Image>
    {


        public override void OnWrite(Writer bw, System.Drawing.Image value)
        {
            var img = value;
            var ms = new System.IO.MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            bw.WriteArray(ms.ToArray());
        }

        public override System.Drawing.Image OnRead(Reader br)
        {
            var ms = new System.IO.MemoryStream(br.ReadArray<byte>());
            return System.Drawing.Image.FromStream(ms);
        }
    }
}
