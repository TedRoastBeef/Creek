using System.Xml;
using Creek.UI.EFML.UI_Elements;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class LabelProcessor : ElementProcessor
    {
        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return "label"; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder b)
        {
            ui = new Label();
        }

        #endregion
    }
}