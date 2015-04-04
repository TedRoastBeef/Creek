using System.Xml;
using Creek.UI.EFML.UI_Elements;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class TabControlProcessor : ElementProcessor
    {
        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return "tab"; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder builder)
        {
            var r = new Tabcontrol();

            foreach (XmlNode childNode in t.ChildNodes)
            {
                if (childNode.Name == "page")
                {
                    var p = new TabPage {Caption = childNode.GetAttributeByName("caption")};
                    builder.UiBaseElement(childNode.ChildNodes, p.Childs);
                    r.Pages.Add(p);
                }
            }

            ui = r;
        }

        #endregion
    }
}