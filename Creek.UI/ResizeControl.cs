using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Creek.UI
{
    public class ResizeControl : IDisposable
    {
        #region "Private Controls"

        private readonly ResizeBox bottomBox;
        private readonly ResizeBox bottomLeftBox;
        private readonly ResizeBox bottomRightBox;
        private readonly ResizeBox leftBox;
        private readonly ResizeBox rightBox;
        private readonly ResizeBox topBox;
        private readonly ResizeBox topLeftBox;
        private readonly ResizeBox topRightBox;

        private class ResizeBox : UserControl
        {
            #region BoxPosition enum

            public enum BoxPosition
            {
                Top,
                Bottom,
                Left,
                Right,
                TopLeft,
                TopRight,
                BottomLeft,
                BottomRight
            }

            #endregion

            private readonly IContainer components = null;

            private BoxPosition _Position = BoxPosition.Top;

            public ResizeBox(BoxPosition position)
            {
                InitializeComponent();
                Position = position;
            }

            public BoxPosition Position
            {
                get { return _Position; }
                set { _Position = value; }
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            private void InitializeComponent()
            {
                SuspendLayout();
                AutoScaleDimensions = new SizeF(6F, 13F);
                AutoScaleMode = AutoScaleMode.Font;
                BackColor = Color.White;
                BorderStyle = BorderStyle.FixedSingle;
                Name = "ResizeBox";
                Size = new Size(6, 6);
                MouseEnter += ResizeBox_MouseEnter;
                MouseLeave += ResizeBox_MouseLeave;
                ResumeLayout(false);
            }

            private void ResizeBox_MouseEnter(object sender, EventArgs e)
            {
                switch (Position)
                {
                    case BoxPosition.Top:
                        Cursor = Cursors.SizeNS;
                        break;
                    case BoxPosition.Bottom:
                        Cursor = Cursors.SizeNS;
                        break;
                    case BoxPosition.Left:
                        Cursor = Cursors.SizeWE;
                        break;
                    case BoxPosition.Right:
                        Cursor = Cursors.SizeWE;
                        break;
                    case BoxPosition.TopLeft:
                        Cursor = Cursors.SizeNWSE;
                        break;
                    case BoxPosition.BottomRight:
                        Cursor = Cursors.SizeNWSE;
                        break;
                    case BoxPosition.TopRight:
                        Cursor = Cursors.SizeNESW;
                        break;
                    case BoxPosition.BottomLeft:
                        Cursor = Cursors.SizeNESW;
                        break;
                    default:
                        Cursor = Cursors.No;
                        break;
                }
            }

            private void ResizeBox_MouseLeave(object sender, EventArgs e)
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region "Public Methods"

        public ResizeControl(Control target, Boolean showResizeBoxes)
        {
            _Target = target;

            target.Parent.Paint += Parent_Paint;

            topBox = new ResizeBox(ResizeBox.BoxPosition.Top);
            bottomBox = new ResizeBox(ResizeBox.BoxPosition.Bottom);
            leftBox = new ResizeBox(ResizeBox.BoxPosition.Left);
            rightBox = new ResizeBox(ResizeBox.BoxPosition.Right);
            topLeftBox = new ResizeBox(ResizeBox.BoxPosition.TopLeft);
            topRightBox = new ResizeBox(ResizeBox.BoxPosition.TopRight);
            bottomLeftBox = new ResizeBox(ResizeBox.BoxPosition.BottomLeft);
            bottomRightBox = new ResizeBox(ResizeBox.BoxPosition.BottomRight);

            topLeftBox.MouseDown += Boxes_MouseDown;
            topBox.MouseDown += Boxes_MouseDown;
            topRightBox.MouseDown += Boxes_MouseDown;
            leftBox.MouseDown += Boxes_MouseDown;
            rightBox.MouseDown += Boxes_MouseDown;
            bottomLeftBox.MouseDown += Boxes_MouseDown;
            bottomBox.MouseDown += Boxes_MouseDown;
            bottomRightBox.MouseDown += Boxes_MouseDown;

            topLeftBox.MouseMove += topLeftBox_MouseMove;
            topBox.MouseMove += topBox_MouseMove;
            topRightBox.MouseMove += topRightBox_MouseMove;
            leftBox.MouseMove += leftBox_MouseMove;
            rightBox.MouseMove += rightBox_MouseMove;
            bottomLeftBox.MouseMove += bottomLeftBox_MouseMove;
            bottomBox.MouseMove += bottomBox_MouseMove;
            bottomRightBox.MouseMove += bottomRightBox_MouseMove;

            if (showResizeBoxes)
                ShowResizeBoxes();
        }

        void IDisposable.Dispose()
        {
            HideResizeBoxes();
        }

        public void Parent_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            var pen = new Pen(Brushes.Black, 1);
            pen.DashStyle = DashStyle.Dot;
            g.DrawRectangle(pen, new Rectangle(Target.Left - 3, Target.Top - 3, Target.Width + 6, Target.Height + 6));
        }

        public void ShowResizeBoxes()
        {
            PositionTopLeftBox();
            PositionTopBox();
            PositionTopRightBox();
            PositionLeftBox();
            PositionRightBox();
            PositionBottomLeftBox();
            PositionBottomBox();
            PositionBottomRightBox();
            Target.Parent.Controls.Add(topBox);
            Target.Parent.Controls.Add(bottomBox);
            Target.Parent.Controls.Add(leftBox);
            Target.Parent.Controls.Add(rightBox);
            Target.Parent.Controls.Add(topLeftBox);
            Target.Parent.Controls.Add(topRightBox);
            Target.Parent.Controls.Add(bottomLeftBox);
            Target.Parent.Controls.Add(bottomRightBox);
        }

        public void HideResizeBoxes()
        {
        }

        #endregion

        #region "Move Event Handlers"

        private Point mouseLocation;

        private void Boxes_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation.X = e.X;
            mouseLocation.Y = e.Y;
        }

        private void topLeftBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Int32 newBoxTop = topLeftBox.Top + (e.Y - mouseLocation.Y);
                Int32 oldTargetTop = Target.Top;
                Int32 newTargetHeight = Target.Height + (Target.Top - (topLeftBox.Top + topLeftBox.Height + 1));
                Int32 newBoxLeft = topLeftBox.Left + (e.X - mouseLocation.X);
                Int32 oldTargetLeft = Target.Left;
                Int32 newTargetWidth = Target.Width + (oldTargetLeft - Target.Left);

                if (newTargetWidth > 30 || newBoxLeft <= topLeftBox.Left)
                {
                    Target.Left = newBoxLeft + topLeftBox.Width + 1;
                    Target.Width += (oldTargetLeft - Target.Left);
                    topLeftBox.Left = newBoxLeft;
                    PositionTopLeftBox();
                    PositionBottomLeftBox();
                    PositionTopBox();
                    PositionBottomBox();
                    PositionLeftBox();
                }
                if (newTargetHeight > 15 || newBoxTop <= topBox.Top)
                {
                    Target.Top = newBoxTop + topLeftBox.Height + 1;
                    Target.Height += (oldTargetTop - Target.Top);
                    topLeftBox.Top = newBoxTop;
                    PositionTopLeftBox();
                    PositionTopRightBox();
                    PositionLeftBox();
                    PositionRightBox();
                    PositionTopBox();
                }
                Target.Parent.Invalidate(new Rectangle(Target.Left - 6, Target.Top - 6, Target.Width + 12,
                                                       Target.Height + 12));
            }
        }

        private void topBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Int32 newBoxTop = topBox.Top + (e.Y - mouseLocation.Y);
                Int32 oldTargetTop = Target.Top;
                Int32 newTargetHeight = Target.Height + (Target.Top - (topBox.Top + topBox.Height + 1));

                if (newTargetHeight > 15 || newBoxTop <= topBox.Top)
                {
                    Target.Top = newBoxTop + topBox.Height + 1;
                    Target.Height += (oldTargetTop - Target.Top);
                    topBox.Top = newBoxTop;
                    PositionTopLeftBox();
                    PositionTopRightBox();
                    PositionLeftBox();
                    PositionRightBox();
                }
                Target.Parent.Invalidate(new Rectangle(Target.Left - 6, Target.Top - 6, Target.Width + 12,
                                                       Target.Height + 12));
            }
        }

        private void topRightBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Int32 newBoxTop = topRightBox.Top + (e.Y - mouseLocation.Y);
                Int32 oldTargetTop = Target.Top;
                Int32 newTargetHeight = Target.Height + (Target.Top - (topRightBox.Top + topRightBox.Height + 1));
                Int32 newBoxLeft = topRightBox.Left + (e.X - mouseLocation.X);
                Int32 newTargetWidth = topRightBox.Left - Target.Left - 1;

                if (newTargetWidth > 30 || newBoxLeft >= topRightBox.Left)
                {
                    Target.Width = newTargetWidth;
                    topRightBox.Left = newBoxLeft;
                    PositionBottomRightBox();
                    PositionTopBox();
                    PositionBottomBox();
                }
                if (newTargetHeight > 15 || newBoxTop <= topRightBox.Top)
                {
                    Target.Top = newBoxTop + topRightBox.Height + 1;
                    Target.Height += (oldTargetTop - Target.Top);
                    topRightBox.Top = newBoxTop;
                    PositionTopLeftBox();
                    PositionLeftBox();
                    PositionRightBox();
                }
                Target.Parent.Invalidate(new Rectangle(Target.Left - 6, Target.Top - 6, Target.Width + 12,
                                                       Target.Height + 12));
            }
        }

        private void leftBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Int32 newBoxLeft = leftBox.Left + (e.X - mouseLocation.X);
                Int32 oldTargetLeft = Target.Left;
                Int32 newTargetWidth = Target.Width + (oldTargetLeft - Target.Left);

                if (newTargetWidth > 30 || newBoxLeft <= leftBox.Left)
                {
                    Target.Left = newBoxLeft + leftBox.Width + 1;
                    Target.Width += (oldTargetLeft - Target.Left);
                    leftBox.Left = newBoxLeft;
                    PositionTopLeftBox();
                    PositionBottomLeftBox();
                    PositionTopBox();
                    PositionBottomBox();
                }
                Target.Parent.Invalidate(new Rectangle(Target.Left - 6, Target.Top - 6, Target.Width + 12,
                                                       Target.Height + 12));
            }
        }

        private void rightBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Int32 newBoxLeft = rightBox.Left + (e.X - mouseLocation.X);
                Int32 newTargetWidth = rightBox.Left - Target.Left - 1;

                if (newTargetWidth > 30 || newBoxLeft >= rightBox.Left)
                {
                    Target.Width = newTargetWidth;
                    rightBox.Left = newBoxLeft;
                    PositionTopRightBox();
                    PositionBottomRightBox();
                    PositionTopBox();
                    PositionBottomBox();
                }
                Target.Parent.Invalidate(new Rectangle(Target.Left - 6, Target.Top - 6, Target.Width + 12,
                                                       Target.Height + 12));
            }
        }

        private void bottomLeftBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Int32 newBoxTop = bottomLeftBox.Top + (e.Y - mouseLocation.Y);
                Int32 newTargetHeight = bottomLeftBox.Top - Target.Top - 1;
                Int32 newBoxLeft = bottomLeftBox.Left + (e.X - mouseLocation.X);
                Int32 oldTargetLeft = Target.Left;
                Int32 newTargetWidth = Target.Width + (oldTargetLeft - Target.Left);

                if (newTargetWidth > 30 || newBoxLeft <= bottomLeftBox.Left)
                {
                    Target.Left = newBoxLeft + bottomLeftBox.Width + 1;
                    Target.Width += (oldTargetLeft - Target.Left);
                    bottomLeftBox.Left = newBoxLeft;
                    PositionTopLeftBox();
                    PositionTopBox();
                    PositionBottomBox();
                }
                if (newTargetHeight > 15 || newBoxTop >= bottomLeftBox.Top)
                {
                    Target.Height = newTargetHeight;
                    bottomLeftBox.Top = newBoxTop;
                    PositionBottomRightBox();
                    PositionLeftBox();
                    PositionRightBox();
                }
                Target.Parent.Invalidate(new Rectangle(Target.Left - 6, Target.Top - 6, Target.Width + 12,
                                                       Target.Height + 12));
            }
        }

        private void bottomBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Int32 newBoxTop = bottomBox.Top + (e.Y - mouseLocation.Y);
                Int32 newTargetHeight = bottomBox.Top - Target.Top - 1;

                if (newTargetHeight > 15 || newBoxTop >= bottomBox.Top)
                {
                    Target.Height = newTargetHeight;
                    bottomBox.Top = newBoxTop;
                    PositionBottomLeftBox();
                    PositionBottomRightBox();
                    PositionLeftBox();
                    PositionRightBox();
                }
                Target.Parent.Invalidate(new Rectangle(Target.Left - 6, Target.Top - 6, Target.Width + 12,
                                                       Target.Height + 12));
            }
        }

        private void bottomRightBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Int32 newBoxTop = bottomRightBox.Top + (e.Y - mouseLocation.Y);
                Int32 newTargetHeight = bottomRightBox.Top - Target.Top - 1;
                Int32 newBoxLeft = bottomRightBox.Left + (e.X - mouseLocation.X);
                Int32 newTargetWidth = bottomRightBox.Left - Target.Left - 1;

                if (newTargetWidth > 30 || newBoxLeft >= bottomRightBox.Left)
                {
                    Target.Width = newTargetWidth;
                    bottomRightBox.Left = newBoxLeft;
                    PositionTopRightBox();
                    PositionTopBox();
                    PositionBottomBox();
                }
                if (newTargetHeight > 15 || newBoxTop >= bottomRightBox.Top)
                {
                    Target.Height = newTargetHeight;
                    bottomRightBox.Top = newBoxTop;
                    PositionBottomLeftBox();
                    PositionLeftBox();
                    PositionRightBox();
                }
                Target.Parent.Invalidate(new Rectangle(Target.Left - 6, Target.Top - 6, Target.Width + 12,
                                                       Target.Height + 12));
            }
        }

        #endregion

        #region "Positioning Commands"

        private void PositionTopLeftBox()
        {
            topLeftBox.Top = Target.Top - topLeftBox.Height - 1;
            topLeftBox.Left = Target.Left - topLeftBox.Width - 1;
        }

        private void PositionTopBox()
        {
            topBox.Top = Target.Top - topBox.Height - 1;
            topBox.Left = Target.Left + (Target.Width/2) - (topBox.Width/2);
        }

        private void PositionTopRightBox()
        {
            topRightBox.Top = Target.Top - topRightBox.Height - 1;
            topRightBox.Left = Target.Left + Target.Width + 1;
        }

        private void PositionLeftBox()
        {
            leftBox.Top = Target.Top + (Target.Height/2) - (leftBox.Height/2);
            leftBox.Left = Target.Left - leftBox.Width - 1;
        }

        private void PositionRightBox()
        {
            rightBox.Top = Target.Top + (Target.Height/2) - (rightBox.Height/2);
            rightBox.Left = Target.Left + Target.Width + 1;
        }

        private void PositionBottomLeftBox()
        {
            bottomLeftBox.Top = Target.Top + Target.Height + 1;
            bottomLeftBox.Left = Target.Left - leftBox.Width - 1;
        }

        private void PositionBottomBox()
        {
            bottomBox.Top = Target.Top + Target.Height + 1;
            bottomBox.Left = Target.Left + (Target.Width/2) - (bottomBox.Width/2);
        }

        private void PositionBottomRightBox()
        {
            bottomRightBox.Top = Target.Top + Target.Height + 1;
            bottomRightBox.Left = Target.Left + Target.Width + 1;
        }

        #endregion

        #region "Properties"

        private Control _Target;

        public Control Target
        {
            get
            {
                if (_Target == null)
                    _Target = new Control();
                return _Target;
            }
        }

        #endregion
    }
}