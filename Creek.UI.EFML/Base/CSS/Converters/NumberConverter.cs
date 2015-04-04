using System;

namespace Creek.UI.EFML.Base.CSS.Converters
{
    public class NumberConverter : IConverter<double>
    {
        #region PixelUnit enum

        public enum PixelUnit
        {
            PX,
            Percent
        }

        #endregion

        public static double Convert(string s, int Content = 0)
        {
            var unit = PixelUnit.PX;
            string u = s.EndsWith("%") ? "Percent" : s.Substring(s.Length - 2, 2);
            if (Enum.IsDefined(typeof (PixelUnit), u.ToUpper()))
                unit = (PixelUnit) Enum.Parse(typeof (PixelUnit), u.ToUpper());
            double v = double.Parse(s.EndsWith("%") ? s.Remove(s.Length - 1, 1) : s.Remove(s.Length - 2, 2));

            switch (unit)
            {
                case PixelUnit.Percent:
                    return Content*v/100;
                case PixelUnit.PX:
                    return v;
            }

            return 0;
        }

        #region Overrides of IConverter<double>

        [Obsolete]
        public override double Convert(string s)
        {
            throw new NotImplementedException();
        }

        public override string Convert(double s)
        {
            return s + "px";
        }

        #endregion
    }
}