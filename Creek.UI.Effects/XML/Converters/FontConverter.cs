using System;
using System.Collections.Generic;
using System.Drawing;

namespace Creek.UI.Effects.XML.Converters
{
    internal class FontConverter : IConverter<Font>
    {

        public new static Font Convert(string s)
        {
            var spl = s.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            var fSize = NumberConverter.Convert(spl[0]);

            var spl1 = new List<string>(spl);
            spl1.RemoveAt(0);

            var fName = string.Join(" ", spl1.ToArray());

            return new Font(fName, (float)fSize);
        }

    }
}
