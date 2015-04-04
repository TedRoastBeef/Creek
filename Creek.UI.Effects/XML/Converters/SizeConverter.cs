using System;
using System.Drawing;

namespace Creek.UI.Effects.XML.Converters
{
    internal class SizeConverter : IConverter<Size>
    {

        public new static Size Convert(string s)
        {
            var split = s.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var r = new Size {Height = (int) NumberConverter.Convert(split[0]), Width = (int) NumberConverter.Convert(split[2])};

            return r;
        }

    }
}
