using System.Xml;
using Creek.UI.EFML.UI_Elements;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class ImageProcessor : ElementProcessor
    {
        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return "image"; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder b)
        {
            var im = new Image {src = t.GetAttributeByName("src")};
            ui = im;
        }

        #endregion
    }
}