using System.IO;
using System.IO.Compression;
using Lib.IO.Internal.Binary;

namespace Creek.Tools
{
    using Creek.Text;

    public class KeyFile
    {
        public string Key { get; set; }

        public void Save(string filename, string pw)
        {
            var bw = new Writer(new GZipStream(new FileStream(filename, FileMode.OpenOrCreate), CompressionMode.Compress));
            var data = StringGenerator.Generate();
            var data2 = StringGenerator.Generate();

            bw.Write<string>(data);
            bw.Write("[" + Encryption.EncryptString(Key, pw) + "/");
            bw.Write<string>(data2);

            bw.Flush();
            bw.Close();
        }
        public void Save(string filename)
        {
            Save(filename, @"E0C0z/GÜ\p#@-LDWümwV-gzmJ{)zkRä$1Q%uj,7_f/:UCT5:Ki%<Jzj8Ü>b!@u2;Q,p3NCdA6JÖÜssFpöH{h%S&HAeS]a>,Ü]RD");
        }

        public static KeyFile Load(string filename, string pw)
        {
            var kf = new KeyFile();
            var br = new Reader(new GZipStream(new FileStream(filename, FileMode.OpenOrCreate), CompressionMode.Decompress));
            br.Read<string>();

            var k = br.Read<string>().To<string>().Remove(0, 1);
            k = k.Remove(k.Length - 1, 1);

            kf.Key = Encryption.DecryptString(k, pw);

            return kf;
        }
        public static KeyFile Load(string filename)
        {
            return Load(filename, @"E0C0z/GÜ\p#@-LDWümwV-gzmJ{)zkRä$1Q%uj,7_f/:UCT5:Ki%<Jzj8Ü>b!@u2;Q,p3NCdA6JÖÜssFpöH{h%S&HAeS]a>,Ü]RD");
        }

    }
}
