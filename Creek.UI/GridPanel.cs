using System;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
    /// <summary>
    /// Grid for Controls with labels.
    /// Depending on Anchor property each Control can be stretched horizontaly.
    /// No vertical stretching is performed.
    /// Name property is used for labeling.
    /// </summary>
    public class GridPanel : UserControl
    {
        /// <summary>
        /// Creates new instance.
        /// </summary>
        public GridPanel(params Control[] controls) : this(null, controls)
        {
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        public GridPanel(string name, params Control[] controls)
        {
            Name = name;
            Controls.AddRange(controls);
        }

        /// <summary>
        /// Preferred Size.
        /// </summary>
        public new Size PreferredSize
        {
            get
            {
                int labelWidth = 0;
                int controlWidth = 0;
                int gridHeight = 0;

                // calc sizes
                Graphics graphics = CreateGraphics();
                foreach (Control control in Controls)
                {
                    if (!control.Visible) continue;

                    // label
                    labelWidth = (int) Math.Max(graphics.MeasureString(control.Name, Font).Width, labelWidth);

                    // control
                    if ((control.Anchor & AnchorStyles.Left) == 0 ||
                        (control.Anchor & AnchorStyles.Right) == 0)
                        controlWidth = Math.Max(control.Width, controlWidth);

                    // grid
                    gridHeight += control.Height + 4;
                }

                // return size
                if (gridHeight > 0)
                    gridHeight -= 4;
                return new Size(labelWidth + controlWidth + 4, gridHeight);
            }
        }

        /// <summary>
        /// Handles OnLayout event.
        /// </summary>
        protected override void OnLayout(LayoutEventArgs e)
        {
            //base.OnLayout(e);

            if (Controls.Count <= 0) return;

            // left
            int left = 0;
            Graphics graphics = CreateGraphics();
            foreach (Control control in Controls)
                if (control.Visible)
                    left = (int) Math.Max(graphics.MeasureString(control.Name, Font).Width, left);
            left += 4;

            // layout
            int top = 0;
            foreach (Control control in Controls)
            {
                if (!control.Visible) continue;

                // top
                control.Top = top;
                top = control.Bottom + 4;

                // left & right
                if ((control.Anchor & AnchorStyles.Left) != 0)
                {
                    if ((control.Anchor & AnchorStyles.Right) != 0)
                        control.Width = ClientSize.Width - left;
                    control.Left = left;
                }
                else
                    control.Left = Math.Max(ClientSize.Width - control.Width, left);
            }
        }

        // OnLayout()

        /// <summary>
        /// Handles OnPaint event.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // draw labels
            Brush brush = new SolidBrush(ForeColor);
            foreach (Control control in Controls)
            {
                if (!control.Visible) continue;

                e.Graphics.DrawString(control.Name, Font, brush, 0, control.Top + 4);
            }
        }

        // PreferredSize
    }
}

// GridPanel{}