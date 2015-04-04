using System.Xml;

namespace Creek.Dynamics.XML.AST
{
    public interface IAst
    {
        void Parse(XmlNode xmlElement, out IAst el);
    }
}
