using System;

namespace Creek.UI.Effects.XML.Converters
{
    internal class NumberConverter : IConverter<double>
    {
        public enum PixelUnit { PX, Percent }

        public static int Convert(string s, int value = 0)
        {
            var unit = PixelUnit.PX;
            var u = s.EndsWith("%") ? "Percent" : s.Substring(s.Length - 2, 2);
            if (Enum.IsDefined(typeof(PixelUnit), u.ToUpper()))
                unit = (PixelUnit)Enum.Parse(typeof(PixelUnit), u.ToUpper());
            var v = int.Parse(s.EndsWith("%") ? s.Remove(s.Length - 1, 1) : s.Remove(s.Length - 2, 2));

            switch (unit)
            {
                case PixelUnit.Percent:
                    return value * v / 100;
                case PixelUnit.PX:
                    return v;
            }

            return 0;
        }
    }
}
