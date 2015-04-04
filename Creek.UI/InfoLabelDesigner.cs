using System.Windows.Forms.Design;

namespace Creek.UI
{
    public class InfoLabelDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get { return base.SelectionRules & ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable); }
        }
    }
}