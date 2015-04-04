using System.Xml;
using Creek.UI.EFML.UI_Elements;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class GroupProcessor : ElementProcessor
    {
        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return "group"; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder builder)
        {
            var r = new Group();

            builder.UiBaseElement(t.ChildNodes, r.Childs);

            r.Caption = t.HasAttribute("caption") ? t.GetAttributeByName("caption") : t.GetAttributeByName("content");

            ui = r;
        }

        #endregion
    }
}