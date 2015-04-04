using System.Drawing;

namespace Creek.UI.Effects.XML.Converters
{
    internal class ColorConverter : IConverter<Color>
    {
        public new static Color Convert(string s)
        {
            if (s.StartsWith("#"))
            {
                return ColorTranslator.FromHtml(s);
            }
            if (Function.IsFunction(s))
            {
                var f = Function.Parse(s);
                if (f.Name == "hsl")
                {
                    var c = new HSLColor
                    {
                        Hue = f.Arg<double>(0),
                        Saturation = f.Arg<double>(1),
                        Luminosity = f.Arg<double>(2)
                    };

                    return c;
                }
                if (f.Name == "hsla")
                {
                    var c = new HSLColor
                    {
                        Hue = f.Arg<double>(0),
                        Saturation = f.Arg<double>(1),
                        Luminosity = f.Arg<double>(2),
                        Alpha = byte.Parse(f.Arg<double>(3).ToString())
                    };

                    return c;
                }
                if (f.Name == "rgb")
                {
                    return Color.FromArgb(f.Arg<byte>(0), f.Arg<byte>(1), f.Arg<byte>(2));
                }
                if (f.Name == "rgba")
                {
                    return Color.FromArgb(f.Arg<byte>(3), f.Arg<byte>(0), f.Arg<byte>(1), f.Arg<byte>(2));
                }
            }
            else if (s != "")
            {
                return Color.FromName(s);
            }
            return default(Color);
        }

    }
}
