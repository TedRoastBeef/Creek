using System;
using System.Drawing;

namespace Creek.UI.EFML.Base.CSS.Converters
{
    public class PointConverter : IConverter<Point>
    {
        public override Point Convert(string s)
        {
            string[] split = s.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var r = new Point {X = (int) NumberConverter.Convert(split[0]), Y = (int) NumberConverter.Convert(split[1])};

            return r;
        }

        public override string Convert(Point s)
        {
            return s.X + "px " + s.Y + "px";
        }
    }
}