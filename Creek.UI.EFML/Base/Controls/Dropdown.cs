using System.Windows.Forms;

namespace Creek.UI.EFML.Base.Controls
{
    public class Dropdown : ComboBox, IUIElement
    {
        #region Implementation of IUIElement

        public string ID { get; set; }

        public string Content
        {
            get { return SelectedItem.ToString(); }
            set { SelectedItem = Content; }
        }

        public IValidator Validator { get; set; }
        public IStyle style { get { return new ControlStyle(this); } }

        #endregion
    }
}