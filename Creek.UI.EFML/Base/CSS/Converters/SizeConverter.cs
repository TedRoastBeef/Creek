using System;
using System.Drawing;

namespace Creek.UI.EFML.Base.CSS.Converters
{
    public class SizeConverter : IConverter<Size>
    {
        public override Size Convert(string s)
        {
            string[] split = s.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var r = new Size
                        {
                            Height = (int) NumberConverter.Convert(split[0]),
                            Width = (int) NumberConverter.Convert(split[2])
                        };

            return r;
        }

        public override string Convert(Size s)
        {
            return s.Height + "px " + s.Width + "px";
        }
    }
}