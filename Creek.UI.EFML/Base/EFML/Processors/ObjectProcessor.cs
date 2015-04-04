using System;
using System.Xml;
using Creek.UI.EFML.UI_Elements;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class ObjectProcessor : ElementProcessor
    {
        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return "object"; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder b)
        {
            ui = new ObjectElement {Type = Type.GetTypeFromProgID(t.GetAttributeByName("progid"))};
        }

        #endregion
    }
}