using System.Xml;

namespace Creek.UI.EFML.Base
{
    public abstract class EventProvider
    {
        public abstract void Resolve(UiElement uiElement, XmlNode markupTag);
    }
}