using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Creek.UI
{
    public class NotificationBar : Control
    {
        private readonly Timer flashTimer = new Timer();
        private int closeButtonPadding = 6;
        private Size closeButtonSize = new Size(20, 20);

        private bool controlHighlighted;

        private int flashCount;
        private int flashTo;
        private int imageKey;
        private bool mouseInBounds;
        private ContextMenuStrip onClickMenu;
        private bool playSoundOnVisible = true;
        private ImageList smallImageList;
        private int tickCount;

        public NotificationBar()
        {
            BackColor = SystemColors.Info;

            flashTimer.Interval = 1000;
            flashTimer.Tick += flashTimer_Tick;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public ContextMenuStrip OnClickMenuStrip
        {
            get { return onClickMenu; }
            set { onClickMenu = value; }
        }

        public ImageList SmallImageList
        {
            get { return smallImageList; }
            set { smallImageList = value; }
        }

        public int ImageIndex
        {
            get { return imageKey; }
            set
            {
                imageKey = value;
                Invalidate();
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        public bool PlaySoundWhenShown
        {
            get { return playSoundOnVisible; }
            set { playSoundOnVisible = value; }
        }

        public void Flash(int interval, int times)
        {
            flashTo = times;
            tickCount = 0;

            flashTimer.Interval = interval;
            flashTimer.Start();
        }

        public void Flash(int numberOfTimes)
        {
            Flash(1000, numberOfTimes);
        }

        public void FlashOnce(int milliseconds)
        {
            Flash(milliseconds, 1);
        }

        private void flashTimer_Tick(object sender, EventArgs e)
        {
            if (controlHighlighted)
            {
                BackColor = SystemColors.Info;
                controlHighlighted = false;
                flashCount++;

                if (flashCount == flashTo)
                {
                    flashTimer.Stop();
                    flashCount = 0;
                }
            }
            else
            {
                BackColor = SystemColors.Highlight;
                controlHighlighted = true;
            }

            tickCount++;
            Invalidate();
        }

        #region Protected Methods

        protected void DrawText(PaintEventArgs e)
        {
            int leftPadding = 1;

            if (smallImageList != null && smallImageList.Images.Count > 0 && smallImageList.Images.Count > imageKey)
            {
                leftPadding = smallImageList.ImageSize.Width + 4;
                e.Graphics.DrawImage(smallImageList.Images[imageKey], new Point(2, 5));
            }

            Size textSize = TextRenderer.MeasureText(e.Graphics, Text, Font);
            int maxTextWidth = (Width - (closeButtonSize.Width + (closeButtonPadding*2)));
            int lineHeight = textSize.Height + 2;
            int numLines = 1;

            if (textSize.Width > maxTextWidth)
            {
                numLines = textSize.Width/maxTextWidth + 1;
            }

            var textRect = new Rectangle();
            textRect.Width = Width - (closeButtonSize.Width + closeButtonPadding) - leftPadding;
            textRect.Height = (numLines*lineHeight);
            textRect.X = leftPadding;
            textRect.Y = 5;

            Height = (numLines*lineHeight) + 10;

            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, ForeColor,
                                  TextFormatFlags.WordBreak | TextFormatFlags.Left | TextFormatFlags.Top);
        }

        protected void DrawCloseButton(PaintEventArgs e)
        {
            Color closeButtonColor = Color.Black;

            if (mouseInBounds)
            {
                closeButtonColor = Color.White;
            }

            var linePen = new Pen(closeButtonColor, 2);
            var line1Start = new Point((Width - (closeButtonSize.Width - closeButtonPadding)), closeButtonPadding);
            var line1End = new Point((Width - closeButtonPadding), (closeButtonSize.Height - closeButtonPadding));
            var line2Start = new Point((Width - closeButtonPadding), closeButtonPadding);
            var line2End = new Point((Width - (closeButtonSize.Width - closeButtonPadding)),
                                     (closeButtonSize.Height - closeButtonPadding));

            e.Graphics.DrawLine(linePen, line1Start, line1End);
            e.Graphics.DrawLine(linePen, line2Start, line2End);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawText(e);
            DrawCloseButton(e);

            base.OnPaint(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            BackColor = SystemColors.Highlight;
            mouseInBounds = true;

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (controlHighlighted)
            {
                BackColor = SystemColors.Highlight;
            }
            else
            {
                BackColor = SystemColors.Info;
            }

            mouseInBounds = false;

            base.OnMouseLeave(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.X >= (Width - (closeButtonSize.Width + closeButtonPadding)) && e.Y <= 12)
            {
                Hide();
            }
            else
            {
                if (onClickMenu != null)
                {
                    onClickMenu.Show(this, e.Location);
                }
            }

            base.OnMouseClick(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible && playSoundOnVisible)
            {
                SystemSounds.Beep.Play();
            }
            base.OnVisibleChanged(e);
        }

        #endregion
    }
}