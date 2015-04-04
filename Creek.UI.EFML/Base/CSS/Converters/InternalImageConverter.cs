using System;
using System.Drawing;

namespace Creek.UI.EFML.Base.CSS.Converters
{
    using Creek.UI.Winforms.EFML.Properties;

    public class InternalImageConverter : IConverter<Image>
    {
        public override Image Convert(string s)
        {
            if (Function.IsFunction(s))
            {
                Function f = Function.Parse(s);
                if (f.Name == "internal")
                {
                    if (f.Arg<string>(0) == "transparent")
                        return Resources.transparent;
                }
            }
            return null;
        }

        public override string Convert(Image s)
        {
            throw new NotImplementedException();
        }
    }
}