using System.Xml;

namespace Creek.UI.EFML.Base
{
    public abstract class ElementProcessor
    {
        public EventProvider EventProvider;

        public abstract string Tagname { get; }
        public abstract void Process(out UiElement ui, XmlNode t, Builder builder);
    }
}