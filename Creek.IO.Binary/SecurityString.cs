using System.Collections;
using System.Collections.Generic;

namespace Creek.IO.Binary
{
    public class SecurityString : Binary<string>, IEnumerable<char>
    {

        private string s;

        public override void OnWrite(Writer bw, string value)
        {
            s = value;
            bw.Write<string>(Encryption.EncryptString(s, "thingrofmedwRFTIEUH786451!=)(/&%$§"));
        }

        public override string OnRead(Reader br)
        {
            var c = br.Read<string>().To<string>();
            try
            {
                s = Encryption.DecryptString(c, "thingrofmedwRFTIEUH786451!=)(/&%$§");
            }
            catch
            {
                s = c;
            }
            return this;
        }

        public static implicit operator string(SecurityString ss)
        {
            return ss.ToString();
        }
        public static implicit  operator SecurityString(string S)
        {
            return new SecurityString { s = S};
        }

        public IEnumerator<char> GetEnumerator()
        {
            return s.GetEnumerator();
        }

        public override string ToString()
        {
            return s;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
