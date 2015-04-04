using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Creek.UI.EFML.Base.EFML.Elements
{
    public class MetaElement : ElementBase
    {
        public string Name;
        public string Content;

        public override void Process(List<ElementBase> Tree, EFMLDocument m)
        {
            Tree.AddRange((from XmlNode mm in m.Meta
                           select new MetaElement
                                      {
                                          Name = mm.GetAttributeByName("name"),
                                          Content = mm.GetAttributeByName("Content")
                                      }));
        }
    }
}