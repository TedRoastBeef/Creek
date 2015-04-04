using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Creek.UI.EFML.Base.EFML.Elements
{
    public class ValidatorElement : ElementBase
    {
        public string Name;
        public string Pattern;

        public override void Process(List<ElementBase> Tree, EFMLDocument m)
        {
            Tree.AddRange((from XmlNode child in m.Head.ChildNodes
                           where child.Name == "validator"
                           select new ValidatorElement
                                      {
                                          Name = child.GetAttributeByName("name"),
                                          Pattern = child.GetAttributeByName("pattern")
                                      }));
        }
    }
}