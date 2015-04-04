//Downloaded from
//Visual C# Kicks - http://vckicks.110mb.com
//The Code Project - http://www.codeproject.com

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Creek.UI.Angle
{
    public partial class AngleSelector : UserControl
    {
        #region Delegates

        public delegate void AngleChangedDelegate();

        #endregion

        private int angle;

        private Rectangle drawRegion;
        private Point origin;

        public AngleSelector()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public int Angle
        {
            get { return angle; }
            set
            {
                angle = value;

                if (!DesignMode && AngleChanged != null)
                    AngleChanged(); //Raise event

                Refresh();
            }
        }

        private void AngleSelector_Load(object sender, EventArgs e)
        {
            setDrawRegion();
        }

        private void AngleSelector_SizeChanged(object sender, EventArgs e)
        {
            Height = Width; //Keep it a square
            setDrawRegion();
        }

        private void setDrawRegion()
        {
            drawRegion = new Rectangle(0, 0, Width, Height);
            drawRegion.X += 2;
            drawRegion.Y += 2;
            drawRegion.Width -= 4;
            drawRegion.Height -= 4;

            int offset = 2;
            origin = new Point(drawRegion.Width/2 + offset, drawRegion.Height/2 + offset);

            Refresh();
        }

        public event AngleChangedDelegate AngleChanged;

        private PointF DegreesToXY(float degrees, float radius, Point origin)
        {
            var xy = new PointF();
            double radians = degrees*Math.PI/180.0;

            xy.X = (float) Math.Cos(radians)*radius + origin.X;
            xy.Y = (float) Math.Sin(-radians)*radius + origin.Y;

            return xy;
        }

        private float XYToDegrees(Point xy, Point origin)
        {
            double angle = 0.0;

            if (xy.Y < origin.Y)
            {
                if (xy.X > origin.X)
                {
                    angle = (xy.X - origin.X)/(double) (origin.Y - xy.Y);
                    angle = Math.Atan(angle);
                    angle = 90.0 - angle*180.0/Math.PI;
                }
                else if (xy.X < origin.X)
                {
                    angle = (origin.X - xy.X)/(double) (origin.Y - xy.Y);
                    angle = Math.Atan(-angle);
                    angle = 90.0 - angle*180.0/Math.PI;
                }
            }
            else if (xy.Y > origin.Y)
            {
                if (xy.X > origin.X)
                {
                    angle = (xy.X - origin.X)/(double) (xy.Y - origin.Y);
                    angle = Math.Atan(-angle);
                    angle = 270.0 - angle*180.0/Math.PI;
                }
                else if (xy.X < origin.X)
                {
                    angle = (origin.X - xy.X)/(double) (xy.Y - origin.Y);
                    angle = Math.Atan(angle);
                    angle = 270.0 - angle*180.0/Math.PI;
                }
            }

            if (angle > 180) angle -= 360; //Optional. Keeps values between -180 and 180
            return (float) angle;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var outline = new Pen(Color.FromArgb(86, 103, 141), 2.0f);
            var fill = new SolidBrush(Color.FromArgb(90, 255, 255, 255));

            PointF anglePoint = DegreesToXY(angle, origin.X - 2, origin);
            var originSquare = new Rectangle(origin.X - 1, origin.Y - 1, 3, 3);

            //Draw
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawEllipse(outline, drawRegion);
            g.FillEllipse(fill, drawRegion);
            g.DrawLine(Pens.Black, origin, anglePoint);

            g.SmoothingMode = SmoothingMode.HighSpeed; //Make the square edges sharp
            g.FillRectangle(Brushes.Black, originSquare);

            fill.Dispose();
            outline.Dispose();

            base.OnPaint(e);
        }

        private void AngleSelector_MouseDown(object sender, MouseEventArgs e)
        {
            int thisAngle = findNearestAngle(new Point(e.X, e.Y));

            if (thisAngle != -1)
            {
                Angle = thisAngle;
                Refresh();
            }
        }

        private void AngleSelector_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                int thisAngle = findNearestAngle(new Point(e.X, e.Y));

                if (thisAngle != -1)
                {
                    Angle = thisAngle;
                    Refresh();
                }
            }
        }

        private int findNearestAngle(Point mouseXY)
        {
            var thisAngle = (int) XYToDegrees(mouseXY, origin);
            if (thisAngle != 0)
                return thisAngle;
            else
                return -1;
        }
    }
}