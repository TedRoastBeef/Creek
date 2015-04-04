using System.Xml;
using Creek.UI.EFML.UI_Elements;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class AudioProcessor : ElementProcessor
    {
        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return "audio"; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder b)
        {
            var uui = new AudioElement
                          {
                              Type = t.GetAttributeByName("type"),
                              Source = t.GetAttributeByName("src"),
                              AutoPlay = t.HasAttribute("autoplay")
                          };
            ui = uui;
        }

        #endregion
    }
}