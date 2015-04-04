using System.Xml;
using Creek.UI.EFML.UI_Elements;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class LineProcessor : ElementProcessor
    {
        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return "line"; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder b)
        {
            ui = new Line();
        }

        #endregion
    }
}