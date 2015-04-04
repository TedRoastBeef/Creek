using System.Xml;
using Creek.UI.EFML.UI_Elements;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class DivProcessor : ElementProcessor
    {
        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return "div"; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder builder)
        {
            var r = new Div();

            builder.UiBaseElement(t.ChildNodes, r.Childs);

            ui = r;
        }

        #endregion
    }
}