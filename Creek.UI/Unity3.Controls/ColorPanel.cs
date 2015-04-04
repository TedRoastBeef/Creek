using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Creek.UI.Unity3.Controls
{
    [DefaultEvent("Click")]
    public class ColorPanel : Label
    {
        private Color _Color;

        private bool _PaintColor = true;

        public ColorPanel()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            BorderStyle = BorderStyle.FixedSingle;
        }

        public Color Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                Invalidate();
            }
        }

        public bool PaintColor
        {
            get { return _PaintColor; }
            set
            {
                _PaintColor = value;
                Invalidate();
            }
        }

        public override bool AutoSize
        {
            get { return false; }
            set { }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_PaintColor || _Color.IsEmpty)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.Clear(BackColor);
                e.Graphics.DrawLine(Pens.Black, 0, 0, ClientSize.Width, ClientSize.Height);
                e.Graphics.DrawLine(Pens.Black, ClientSize.Width, 0, 0, ClientSize.Height);
                return;
            }

            if (_Color.A != 255)
            {
                bool b = false;
                var r = new Rectangle(0, 0, 8, 8);
                e.Graphics.Clear(Color.White);
                for (r.Y = 0; r.Y < Height; r.Y += 8)
                    for (r.X = ((b = !b) ? 0 : 8); r.X < Width; r.X += 16)
                        e.Graphics.FillRectangle(Brushes.LightGray, r);
            }

            using (var br = new SolidBrush(_Color))
            {
                e.Graphics.FillRectangle(br, ClientRectangle);
            }
        }
    }
}