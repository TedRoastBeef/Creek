using System.Xml;
using Creek.UI.EFML.UI_Elements;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class DropDownProcessor : ElementProcessor
    {
        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return UI.EFML.Global.TagnameProvider[Tag.Dropdown]; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder builder)
        {
            var r = new Dropdown();
            new ListEventProvider().Resolve(r, t);

            if (t.ChildNodes.Count > 0)
            {
                foreach (XmlNode child in t.ChildNodes)
                {
                    if (child.Name == "item")
                    {
                        r.Childs.Add(child.GetAttributeByName("value"));
                    }
                }
            }

            ui = r;
        }

        #endregion
    }
}