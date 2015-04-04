using System.Linq;
using System.Xml;

namespace Creek.UI.EFML
{
    public static class Extensions
    {
        public static bool HasAttribute(this XmlNode x, string name)
        {
            return x.Attributes.Cast<XmlAttribute>().Any(a => a.Name == name);
        }

        public static string GetAttributeByName(this XmlNode x, string name)
        {
            foreach (XmlAttribute a in from XmlAttribute a in x.Attributes where a.Name == name select a)
            {
                return a.Value;
            }
            return "";
        }
    }
}