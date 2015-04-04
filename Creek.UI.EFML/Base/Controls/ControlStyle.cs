using System.Drawing;
using System.Windows.Forms;
using Creek.UI.Effects;

namespace Creek.UI.EFML.Base.Controls
{
    public class ControlStyle : IStyle {
        #region Implementation of IStyle

        private Control c;
        public ControlStyle(Control c)
        {
            this.c = c;
        }

        public bool autosize { get { return c.AutoSize; } set { c.AutoSize = value; } }
        public Font font { get { return c.Font; } set { c.Font = value; } }
        public Size Size { get { return c.Size; } set { c.Size = value; } }
        public Padding margin { get { return c.Margin; } set { c.Margin = value; } }
        public Padding padding { get { return c.Padding; } set { c.Padding = value; } }
        public Transition transition { get; set; }

        #endregion
    }
}