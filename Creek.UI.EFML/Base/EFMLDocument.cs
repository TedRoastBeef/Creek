using System.IO;
using System.Xml;

namespace Creek.UI.EFML.Base
{
    public class EFMLDocument
    {
        private readonly XmlDocument doc = new XmlDocument();

        public XmlNode Head
        {
            get { return doc.GetElementsByTagName("head")[0]; }
        }

        public XmlNode Body
        {
            get { return doc.GetElementsByTagName("body")[0]; }
        }

        public XmlNodeList Meta
        {
            get { return doc.GetElementsByTagName("meta"); }
        }

        public XmlNodeList Stylesheets
        {
            get { return doc.GetElementsByTagName("style"); }
        }

        public XmlNodeList Scripts
        {
            get { return doc.GetElementsByTagName("script"); }
        }

        public static EFMLDocument Load(string efml)
        {
            var r = new EFMLDocument();
            if (efml.StartsWith("???"))
                efml = efml.Remove(0, 3);
            r.doc.Load(new StringReader(efml));
            return r;
        }
    }
}