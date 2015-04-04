using System.Xml;
using Creek.UI.EFML.Base;

namespace Creek.UI.EFML.UI_Elements.EventProviders
{
    public class CheckBoxEventProvider : EventProvider
    {
        public override void Resolve(UiElement e, XmlNode markupTag)
        {
            if (markupTag.HasAttribute("oncheckedchanged"))
                e.Events["oncheckedchanged"] = markupTag.GetAttributeByName("oncheckedchanged");
        }
    }
}