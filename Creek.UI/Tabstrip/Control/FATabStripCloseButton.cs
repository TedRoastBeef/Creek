using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI.Tabstrip.Control
{
    internal class FATabStripCloseButton
    {
        #region Fields

        private readonly ToolStripProfessionalRenderer renderer;
        private Rectangle crossRect = Rectangle.Empty;
        private bool isMouseOver;

        #endregion

        #region Props

        public bool IsMouseOver
        {
            get { return isMouseOver; }
            set { isMouseOver = value; }
        }

        public Rectangle Bounds
        {
            get { return crossRect; }
            set { crossRect = value; }
        }

        #endregion

        #region Ctor

        internal FATabStripCloseButton(ToolStripProfessionalRenderer renderer)
        {
            this.renderer = renderer;
        }

        #endregion

        #region Methods

        public void DrawCross(Graphics g)
        {
            if (isMouseOver)
            {
                Color fill = renderer.ColorTable.ButtonSelectedHighlight;

                g.FillRectangle(new SolidBrush(fill), crossRect);

                Rectangle borderRect = crossRect;

                borderRect.Width--;
                borderRect.Height--;

                g.DrawRectangle(SystemPens.Highlight, borderRect);
            }

            using (var pen = new Pen(Color.Black, 1.6f))
            {
                g.DrawLine(pen, crossRect.Left + 3, crossRect.Top + 3,
                           crossRect.Right - 5, crossRect.Bottom - 4);

                g.DrawLine(pen, crossRect.Right - 5, crossRect.Top + 3,
                           crossRect.Left + 3, crossRect.Bottom - 4);
            }
        }

        #endregion
    }
}