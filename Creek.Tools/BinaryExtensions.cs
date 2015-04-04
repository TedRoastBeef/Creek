namespace Creek.Tools
{
    using Creek.Drawing;

    public static class BinaryExtensions
    {
        public static string ToBinString(this int value)
        {
            Bin32 rtn = value;
            return rtn.ToString();
        }

        public static string ToOctString(this int value)
        {
            Oct32 rtn = value;
            return rtn.ToString();
        }

        public static string ToHexString(this int value)
        {
            Hex32 rtn = value;
            return rtn.ToString();
        }

        public static int FromBinString(this string value)
        {
            Bin32 rtn = value;
            return rtn;
        }

        public static int FromOctString(this string value)
        {
            Oct32 rtn = value;
            return rtn;
        }

        public static int FromHexString(this string value)
        {
            Hex32 rtn = value;
            return rtn;
        }
    }
}