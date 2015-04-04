using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI.EFML.Base.Controls
{
    public class DivPanel : Panel, IUIElement
    {
        #region Implementation of IUIElement

        public string ID { get; set; }
        public string Content { get; set; }

        public IValidator Validator { get; set; }
        public IStyle style { get { return new ControlStyle(this); } }

        #endregion
    }
}