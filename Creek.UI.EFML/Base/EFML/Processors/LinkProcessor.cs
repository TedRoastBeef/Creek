using System.Xml;
using Creek.UI.EFML.UI_Elements;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class LinkProcessor : ElementProcessor
    {
        public LinkProcessor()
        {
            EventProvider = new LinkEventProvider();
        }

        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return "link"; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder b)
        {
            var eui = new Link {Href = t.GetAttributeByName("href")};

            EventProvider.Resolve(eui, t);
            ui = eui;
        }

        #endregion
    }
}