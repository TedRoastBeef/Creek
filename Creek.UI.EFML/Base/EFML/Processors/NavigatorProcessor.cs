using System.Xml;
using Creek.UI.EFML.UI_Elements;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class NavigatorProcessor : ElementProcessor
    {
        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return "nav"; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder builder)
        {
            var r = new Navigator();

            foreach (XmlNode p in t.ChildNodes)
            {
                var pp = new Page {Caption = p.GetAttributeByName("caption")};
                builder.UiBaseElement(p.ChildNodes, pp.Childs);
                r.Pages.Add(pp);
            }
            ui = r;
        }

        #endregion
    }
}