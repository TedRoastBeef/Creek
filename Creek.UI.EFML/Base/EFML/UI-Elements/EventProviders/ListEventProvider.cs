using System.Xml;
using Creek.UI.EFML;
using Creek.UI.EFML.Base;

public class ListEventProvider : EventProvider
{
    public override void Resolve(UiElement e, XmlNode markupTag)
    {
        if (markupTag.HasAttribute("onselectionchange"))
            e.Events["onselectionchange"] = markupTag.GetAttributeByName("onselectionchange");
    }
}