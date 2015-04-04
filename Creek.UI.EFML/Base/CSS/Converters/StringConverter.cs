namespace Creek.UI.EFML.Base.CSS.Converters
{
    public class StringConverter : IConverter<char[]>
    {
        public override char[] Convert(string s)
        {
            string r = s;

            if (r.StartsWith("'"))
                r = r.Remove(0, 1);
            if (r.EndsWith("'"))
                r = r.Remove(r.Length - 1, 1);

            return r.ToCharArray();
        }

        public override string Convert(char[] s)
        {
            return "'" + string.Join("", s) + "'";
        }

        public string ToString(char[] c)
        {
            return string.Join("", c);
        }
    }
}