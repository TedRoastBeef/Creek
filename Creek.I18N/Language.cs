using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Creek.I18N.Internal;

namespace Creek.I18N
{
    public class Language
    {
        internal Dictionary<string, string> Values;

        public Language()
        {
            Values = new Dictionary<string, string>();
        }

        public Language Load(byte[] b)
        {
            Load(new MemoryStream(b));
            return this;
        }
        public Language Load(Stream s)
        {
            var br = new Reader(s);
            Values = br.Read<Dictionary<string, string>>().To<Dictionary<string, string>>();
            br.Close();
            return this;
        }
        public Language Load(string filename)
        {
            Load(new FileStream(filename, FileMode.OpenOrCreate));
            return this;
        }

        public Language Save(out byte[] oBytes)
        {
            var formatter = new BinaryFormatter();
            var memStream = new MemoryStream();

            formatter.Serialize(memStream, Values);

            Save(memStream);
            oBytes = memStream.ToArray();
            return this;
        }
        public Language Save(Stream s)
        {
            var bw = new Writer(s);
            bw.Write<Dictionary<string, string>>(Values);
            bw.Flush();
            bw.Close();
            return this;
        }
        public Language Save(string filename)
        {
            Save(new FileStream(filename, FileMode.OpenOrCreate));
            return this;
        }

        public static Language Create(Dictionary<string, string> values)
        {
            return new Language {Values = values};
        }

        public static Language Parse(string s)
        {
            var ret = new Language();
            var spl = s.Split('\r');
            foreach (var kv in spl.Select(s1 => s1.Split('=')))
            {
                ret.Values.Add(kv[0], kv[1]);
            }
            return ret;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var value in Values)
            {
                sb.AppendLine(value.Key + "=" + value.Value);
            }

            return sb.ToString();
        }

    }
}
