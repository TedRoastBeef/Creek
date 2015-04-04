using System.Xml;
using Creek.UI.EFML;
using Creek.UI.EFML.Base;

public class DefaultEventProvider : EventProvider
{
    public override void Resolve(UiElement e, XmlNode markupTag)
    {
        if (markupTag.HasAttribute("onclick"))
            e.Events["onclick"] = markupTag.GetAttributeByName("onclick");
        if (markupTag.HasAttribute("onhover"))
            e.Events["onhover"] = markupTag.GetAttributeByName("onhover");
        if (markupTag.HasAttribute("onclick"))
            e.Events["onleave"] = markupTag.GetAttributeByName("onleave");
    }
}