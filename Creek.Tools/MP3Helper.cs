using System;
using System.IO;

namespace Creek.Tools
{
    public class MP3Helper
    {
        public static void Play(string MP3_FileName,bool Repeat)
        {
            API.mciSendString("open \"" + MP3_FileName + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
            API.mciSendString("play MediaFile" + (Repeat ? " repeat" : String.Empty), null, 0, IntPtr.Zero);
        }
        public static void Play(byte[] MP3_EmbeddedResource, bool Repeat)
        {
            extractResource(MP3_EmbeddedResource, Path.GetTempPath() + "resource.tmp");
            API.mciSendString("open \"" + Path.GetTempPath() + "resource.tmp" + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
            API.mciSendString("play MediaFile" + (Repeat ? " repeat" : String.Empty), null, 0, IntPtr.Zero);
        }

        public static void Pause()
        {
            API.mciSendString("stop MediaFile", null, 0, IntPtr.Zero);
        }

        public static void Stop()
        {
            API.mciSendString("close MediaFile", null, 0, IntPtr.Zero);
        }

        private static void extractResource(byte[] res,string filePath)
        {
            FileStream fs;
            BinaryWriter bw;

            if (!File.Exists(filePath))
            {
                fs = new FileStream(filePath, FileMode.OpenOrCreate);
                bw = new BinaryWriter(fs);

                foreach (byte b in res)
                    bw.Write(b);

                bw.Close();
                fs.Close();
            }
        }
    }
}
