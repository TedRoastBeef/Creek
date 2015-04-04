using System.Xml;
using Creek.UI.EFML;
using Creek.UI.EFML.Base;

public class LinkEventProvider : EventProvider
{
    public override void Resolve(UiElement e, XmlNode markupTag)
    {
        if (markupTag.HasAttribute("onclick"))
            e.Events["onclick"] = markupTag.GetAttributeByName("onclick");
    }
}