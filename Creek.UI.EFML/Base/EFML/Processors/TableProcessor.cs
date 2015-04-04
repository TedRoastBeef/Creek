using System.Xml;
using Creek.UI.EFML.UI_Elements;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class TableProcessor : ElementProcessor
    {
        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return "table"; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder builder)
        {
            var r = new Table();

            /* var columns = t.ChildNodes[0].ChildNodes;
            var rows = t.ChildNodes[1].ChildNodes;

            builder.UiBaseElement(rows, r.Rows);
            builder.UiBaseElement(columns, r.Columns);
            */
            ui = r;
        }

        #endregion
    }
}