using System.Xml;
using Creek.UI.EFML;
using Creek.UI.EFML.Base;

public class TextEventProvider : EventProvider
{
    public override void Resolve(UiElement e, XmlNode markupTag)
    {
        if (markupTag.HasAttribute("ontextchanged"))
            e.Events["ontextchanged"] = markupTag.GetAttributeByName("ontextchanged");
    }
}