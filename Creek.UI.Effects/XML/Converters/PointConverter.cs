using System;
using System.Drawing;

namespace Creek.UI.Effects.XML.Converters
{
    internal class PointConverter : IConverter<Point>
    {

        public new static Point Convert(string s)
        {
            var split = s.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var r = new Point {X = (int) NumberConverter.Convert(split[0]), Y = (int) NumberConverter.Convert(split[1])};

            return r;
        }

    }
}
