//Downloaded from
//Visual C# Kicks - http://vckicks.110mb.com
//The Code Project - http://www.codeproject.com

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Creek.UI.Angle
{
    public sealed partial class AngleAltitudeSelector : UserControl
    {
        #region Delegates

        public delegate void AltitudeChangedDelegate();

        public delegate void AngleChangedDelegate();

        #endregion

        private int altitude = 90;
        private int angle;

        private Rectangle drawRegion;
        private Point origin;

        public AngleAltitudeSelector()
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

        public int Altitude
        {
            get { return altitude; }
            set
            {
                altitude = value;

                if (!DesignMode && AltitudeChanged != null)
                    AltitudeChanged(); //Raise event

                Refresh();
            }
        }

        private void AngleAltitudeSelector_Load(object sender, EventArgs e)
        {
            setDrawRegion();
        }

        private void AngleAltitudeSelector_SizeChanged(object sender, EventArgs e)
        {
            Height = Width;
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

        public event AltitudeChangedDelegate AltitudeChanged;

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

        private float getDistance(Point point1, Point point2)
        {
            return (float) Math.Sqrt(Math.Pow((point1.X - point2.X), 2) + Math.Pow((point1.Y - point2.Y), 2));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var outline = new Pen(Color.FromArgb(86, 103, 141), 2.0f);
            var fill = new SolidBrush(Color.FromArgb(90, 255, 255, 255));

            float radius = (origin.X*(90.0f - altitude)/100.0f);

            PointF anglePoint = DegreesToXY(angle, radius, origin);
            var originSquare = new Rectangle(origin.X - 1, origin.Y - 1, 3, 3);
            var pointSquare = new Rectangle((int) anglePoint.X, (int) anglePoint.Y, 1, 1);

            //Draw
            g.SmoothingMode = SmoothingMode.AntiAlias; //Smooth edges

            g.DrawEllipse(outline, drawRegion);
            g.FillEllipse(fill, drawRegion);

            g.SmoothingMode = SmoothingMode.HighSpeed; //Make the edges sharp

            //Draw point
            g.FillRectangle(Brushes.Black, pointSquare);

            int leftX0 = pointSquare.X - 3;
            if (leftX0 < 0) leftX0 = 0;

            int leftX = pointSquare.X - 2;
            if (leftX < 0) leftX = 0;

            int rightX0 = pointSquare.X + 2;
            if (rightX0 > drawRegion.Right) rightX0 = drawRegion.Right;

            int rightX = pointSquare.X + 3;
            if (rightX > drawRegion.Right) rightX = drawRegion.Right;

            int topY0 = pointSquare.Y - 3;
            if (topY0 < 0) topY0 = 0;

            int topY = pointSquare.Y - 2;
            if (topY < 0) topY = 0;

            int bottomY0 = pointSquare.Y + 2;
            if (bottomY0 > drawRegion.Bottom) bottomY0 = drawRegion.Bottom;

            int bottomY = pointSquare.Y + 3;
            if (bottomY > drawRegion.Bottom) bottomY = drawRegion.Bottom;

            g.DrawLine(Pens.Black, leftX0, pointSquare.Y, leftX, pointSquare.Y);
            g.DrawLine(Pens.Black, rightX0, pointSquare.Y, rightX, pointSquare.Y);

            g.DrawLine(Pens.Black, pointSquare.X, topY0, pointSquare.X, topY);
            g.DrawLine(Pens.Black, pointSquare.X, bottomY0, pointSquare.X, bottomY);

            g.FillRectangle(Brushes.Black, originSquare);

            fill.Dispose();
            outline.Dispose();

            base.OnPaint(e);
        }

        private void AngleAltitudeSelector_MouseDown(object sender, MouseEventArgs e)
        {
            int thisAngle = findNearestAngle(new Point(e.X, e.Y));
            int thisAltitude = findAltitude(new Point(e.X, e.Y));

            if (thisAngle != -1)
                Angle = thisAngle;

            Altitude = thisAltitude;

            Refresh();
        }

        private void AngleAltitudeSelector_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                int thisAngle = findNearestAngle(new Point(e.X, e.Y));
                int thisAltitude = findAltitude(new Point(e.X, e.Y));

                if (thisAngle != -1)
                    Angle = thisAngle;

                Altitude = thisAltitude;

                Refresh();
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

        private int findAltitude(Point mouseXY)
        {
            float distance = getDistance(mouseXY, origin);
            int alt = 90 - (int) (90.0f*(distance/origin.X));
            if (alt < 0) alt = 0;

            return alt;
        }
    }
}