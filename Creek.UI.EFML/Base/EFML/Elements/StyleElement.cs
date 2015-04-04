using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Creek.UI.EFML.Base.EFML.Elements
{
    public class StyleElement : ElementBase
    {
        public string Source;

        public override void Process(List<ElementBase> Tree, EFMLDocument m)
        {
            Tree.AddRange((from XmlNode s in m.Stylesheets
                           select new StyleElement
                                      {
                                          Source =
                                              s.HasAttribute("src")
                                                  ? File.ReadAllText(s.GetAttributeByName("src"))
                                                  : s.InnerText
                                      }));
        }
    }
}