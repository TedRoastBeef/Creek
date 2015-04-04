using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace Creek.UI.EFML.Base.CSS.Converters
{
    public class ImageConverter : IConverter<Image>
    {
        public override Image Convert(string s)
        {
            Stream Str = null;
            if (Function.IsFunction(s))
            {
                Function f = Function.Parse(s);
                if (f.Name == "url")
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(f.Arg<string>(0));
                    HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    Str = httpWebReponse.GetResponseStream();
                }
                if (f.Name == "path")
                {
                    Str = new FileStream(f.Arg<string>(0), FileMode.OpenOrCreate);
                }
            }
            return Image.FromStream(Str);
        }

        public override string Convert(Image s)
        {
            throw new NotImplementedException();
        }
    }
}