using System.Drawing;
using System.IO;
using System.Net;

namespace Creek.UI.Effects.XML.Converters
{
    internal class ImageConverter : IConverter<Image>
    {

        public new static Image Convert(string s)
        {
            Stream Str = null;
            if(Function.IsFunction(s))
            {
                var f = Function.Parse(s);
                if(f.Name=="url")
                {
                    var request = WebRequest.Create(f.Arg<string>(0));

                    using (var response = request.GetResponse())
                        using (var stream = response.GetResponseStream())
                        {
                            Str = stream;
                        }
                    request.Abort();
                }
                if(f.Name == "path")
                {
                    Str = new FileStream(f.Arg<string>(0), FileMode.OpenOrCreate);
                }
            }
            return Image.FromStream(Str);
        }

    }
}
