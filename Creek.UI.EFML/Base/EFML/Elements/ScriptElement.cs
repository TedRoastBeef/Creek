using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Creek.UI.EFML.Base.EFML.Elements
{
    public class ScriptElement : ElementBase
    {
        public string Source;

        public override void Process(List<ElementBase> Tree, EFMLDocument m)
        {
            Tree.AddRange((from XmlNode script in m.Scripts
                           select new ScriptElement
                                      {
                                          Source =
                                              script.HasAttribute("src")
                                                  ? File.ReadAllText(script.GetAttributeByName("src"))
                                                  : script.InnerText
                                      }));
        }
    }
}