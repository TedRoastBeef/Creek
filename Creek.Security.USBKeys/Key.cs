using System;

namespace Creek.Security.USBKeys
{
    public class Key
    {
        internal string PW;

        public override string ToString()
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(PW));
        }

        public static implicit operator string(Key k)
        {
            return k.PW;
        }

        public static Key From(string pw)
        {
            var r = new Key {PW = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pw))};
            return r;
        }
    }
}
